# UnityWork
 这是一个unity简易框架  主要特点：  
 1.事件注册 消息注册 采用标记形式![image](https://github.com/huangjunxiang1/UnityWork/assets/88924054/4519310c-b94b-4c61-aa07-9438948ac654)
底层自动化注册 自动化取消  如果函数标记为静态 则启动app注册 永不取消  
2.引入fairyGui 作为ui开发框架  也加入了ugui的支持  UI代码绑定采用代码生成 简化部分UI开发  
3.也有ecs组合式开发 本框架是oop和ecs的结合  每个oop都从SObject派生  如果需要 你也可以给任何SObject添加一些功能组件 从理解上SObject=unity的GameObject  组件=MonoBehavior 也有Awake Update Dispose生命周期 唯一不同的  是组件数值变化 做了方便处理 面向函数编程![image](https://github.com/huangjunxiang1/UnityWork/assets/88924054/554ad4f8-39eb-48b2-a811-e87bb10a8dae) 如上  Awake Update Dispose 可以自定义多个静态函数处理 可以把处理函数放在任何地方  change标记的函数  他的参数组件 只要任何一个组件变化  都会执行change标记函数（函数参数组件 同一个SObject上都有这些组件才会执行） 组件变化事件  需要你对组件赋值或者改值之后 调用一下组件的 SComponent.Change 函数 底层会自动匹配 哪些处理函数需要调用  
4.导表工具和协议工具的生成代码  具体看工程内

