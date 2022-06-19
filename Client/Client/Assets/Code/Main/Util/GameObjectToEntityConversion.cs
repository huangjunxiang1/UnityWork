using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameObjectToEntityConversion : ConvertToEntity, IConvertGameObjectToEntity
{
	private TaskAwaiter<Entity> converTask = null;    // 转换结束时的回调

	// 重写Awake，不走创建Component时，立马转Entity的流程
	private void Awake()
	{
	}

	public void StartConvert(Mode mode, TaskAwaiter<Entity> task = null)
	{
		ConversionMode = mode;
		converTask = task;

		if (World.DefaultGameObjectInjectionWorld != null)
		{
			var system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ConvertToEntitySystem>();
			system.AddToBeConverted(World.DefaultGameObjectInjectionWorld, this);
		}
		else
			UnityEngine.Debug.LogWarning($"{nameof(ConvertToEntity)} failed because there is no {nameof(World.DefaultGameObjectInjectionWorld)}", this);
	}

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem converstionSystem)
	{
		converTask.TrySetResult(entity); //外部的转换结束回调
	}


	public static TaskAwaiter<Entity> ConverToEntity(GameObject target)
	{
		TaskAwaiter<Entity> task = new TaskAwaiter<Entity>();
		target.AddComponent<GameObjectToEntityConversion>().StartConvert(Mode.ConvertAndInjectGameObject, task);
		return task;
	}
	public static TaskAwaiter<Entity> ConverToEntity(GameObject target, TaskAwaiter<Entity> task)
	{
		target.AddComponent<GameObjectToEntityConversion>().StartConvert(Mode.ConvertAndInjectGameObject, task);
		return task;
	}
}