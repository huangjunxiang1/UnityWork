using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

enum EnemyKind { Normal, Charger, Shooter, Tank, Boss }
enum LevelMod { None, Swarm, Hardened, Swift, GlassCannon, BloodMoon }
enum MapKind { Obstacle, Portal, EnergyPillar, Mine }
enum WeaponKind { MultiArrow = 0, Shotgun = 1, Boomerang = 2, Explosive = 3 }

[Scene("Virus")]
class VirusScene : Scene
{
    const float WORLD_H = 30f;
    float _worldW = 53f;

    const float PLAYER_BASE_HP = 150f;
    const float PLAYER_BASE_SPEED = 12f;
    const float PLAYER_BASE_DMG = 10f;
    const float PLAYER_BASE_FIRE_RATE = 2.2f;
    const float PLAYER_MAX_FIRE_RATE = 5.0f;
    const float PLAYER_BASE_RADIUS = 0.8f;
    const float PLAYER_BASE_RANGE = 18f;
    const float PLAYER_MAX_RANGE = 45f;
    const int MAX_SIDES = 10;

    const float SPAWN_MARGIN = 1.5f;
    const float DROP_CHANCE_BASE = 0.25f;
    const float DROP_CHANCE_PER_LV = 0.01f;
    const float DROP_CHANCE_MAX = 0.5f;
    const float BASE_ATTRACT_RADIUS = 5f;
    const float PICKUP_RADIUS = 0.7f;
    const float BAR_WORLD_HEIGHT = 0.16f;
    const float BAR_WORLD_WIDTH_FACTOR = 1.8f;

    const float BULLET_SPEED_MAX = 55f;
    const float PLAYER_BULLET_SPEED_INIT = 40f;
    const float ENEMY_BULLET_SPEED = 24f;
    const float ENEMY_BULLET_RANGE = 22f;
    const int MAX_BOUNCE = 3;

    const float PK_DMG_ADD = 1.5f;
    const float PK_SPEED_ADD = 0.4f;
    const float PK_FIRE_RATE_ADD = 0.12f;
    const float PK_HP_ADD = 15f;
    const float PK_BULLET_SPD_ADD = 4f;
    const float PK_ATTRACT_ADD = 0.5f;
    const float PK_HEAL_AMOUNT = 15f;
    const float PK_DR_ADD = 0.03f;
    const float PK_REGEN_ADD = 0.5f;
    const float PK_RANGE_ADD = 2.5f;
    const float PK_KNOCKBACK_ADD = 0.8f;
    const float MAX_DAMAGE_REDUCE = 0.4f;

    const float BURN_DPS_BASE = 4f;
    const float BURN_DPS_PER_LEVEL = 2f;
    const float BURN_DURATION = 2.5f;
    const float SLOW_FACTOR_BASE = 0.7f;
    const float SLOW_FACTOR_PER_LEVEL = 0.04f;
    const float SLOW_FACTOR_MIN = 0.25f;
    const float SLOW_DURATION = 2.0f;

    const float EXP_UPGRADE_SIDES_HP = 8f;
    const float EXP_UPGRADE_HEAL = 8f;
    const float DASH_DURATION = 0.16f;
    const float DASH_SPEED_MUL = 4.2f;
    const float DASH_CD = 1.5f;
    const float DASH_INVULN = 0.25f;
    const float COMBO_TIMEOUT = 2.5f;
    const float COMBO_PER_STACK = 0.10f;
    const float COMBO_MAX_MUL = 3.5f;
    const int MAX_ENEMY_SIDES = 9;
    const int BOSS_EVERY = 5;
    const int BOSS_SIDES = 20;
    const float BOSS_RADIUS = 2.0f;

    const int SYNERGY_FIRE_LEVEL = 3;
    const int SYNERGY_ICE_LEVEL = 3;
    const int SYNERGY_BOUNCE = 3;
    const float SYNERGY_FIRE_RATE = 3f;
    const float SYNERGY_DR = 0.30f;
    const float SYNERGY_REGEN = 2f;
    const float SYNERGY_EVAPORATE_RADIUS = 4f;
    const float SYNERGY_EVAPORATE_DMG = 40f;
    const float SYNERGY_THORNS_RADIUS = 1.8f;
    const float SYNERGY_THORNS_DPS = 15f;

    const float BURST_CD = 12f;
    const float BURST_RADIUS = 8f;
    const float BURST_DMG = 80f;
    const float BURST_PUSH = 8f;
    const float TIMESTOP_CD = 20f;
    const float TIMESTOP_DURATION = 3f;
    const float TIMESTOP_SLOW = 0.2f;

    const int WEAPON_COUNT = 4;
    const float SHOTGUN_SPREAD = 0.28f;
    const float BOOMERANG_MAX_DIST = 12f;
    const float EXPLOSIVE_RADIUS = 3.5f;
    const float EXPLOSIVE_DMG_MUL = 0.7f;

    const float OBSTACLE_RADIUS = 1.0f;
    const float PORTAL_RADIUS = 0.9f;
    const float PILLAR_RADIUS = 1.6f;
    const float PILLAR_HEAL_RATE = 6f;
    const float MINE_RADIUS = 0.7f;
    const float MINE_BLAST_RADIUS = 3.5f;
    const float MINE_DMG = 30f;

    const int PK_EXP = 0;
    const int PK_DMG = 1;
    const int PK_SPEED = 2;
    const int PK_FIRE = 3;
    const int PK_HP = 4;
    const int PK_BULLET_SPD = 5;
    const int PK_ATTRACT = 6;
    const int PK_HEAL = 7;
    const int PK_BOUNCE = 8;
    const int PK_DR = 9;
    const int PK_REGEN = 10;
    const int PK_FIRE_AMMO = 11;
    const int PK_ICE_AMMO = 12;
    const int PK_WEAPON = 13;
    const int PK_RANGE = 14;
    const int PK_KNOCKBACK = 15;

    Transform _root;
    VirusHUD _hud;
    Transform _camTarget;
    CinemachineCamera _vcam;
    int _lastScreenW = -1;
    int _lastScreenH = -1;

    Unit _player;
    readonly List<Unit> _enemies = new List<Unit>();
    readonly List<Arrow> _arrows = new List<Arrow>();
    readonly List<Pickup> _pickups = new List<Pickup>();
    readonly List<Unit> _killBuffer = new List<Unit>();
    readonly List<MapObject> _mapObjects = new List<MapObject>();
    readonly List<Arrow> _bulletCollisionBuffer = new List<Arrow>();   // 用于子弹抵消收集

    int _level = 0;
    int _enemiesToSpawn;
    float _spawnTimer;
    float _levelBreakTimer;
    bool _inLevelBreak;
    bool _isBossLevel;
    bool _skipNextLevelIncrement;
    Unit _boss;
    int _bossPhase;
    LevelMod _currentMod = LevelMod.None;

    float _playerHp, _playerMaxHp, _playerSpeed, _playerDamage, _playerFireRate;
    int _playerSides;
    float _playerRadius;
    float _bulletSpeed = PLAYER_BULLET_SPEED_INIT;
    float _attractRadius = BASE_ATTRACT_RADIUS;
    int _bounceCount = 0;
    float _damageReduce = 0f;
    float _hpRegen = 0f;
    int _fireLevel = 0;
    int _iceLevel = 0;
    float _playerAttackRange = PLAYER_BASE_RANGE;
    float _playerKnockback = 0f;

    int _currentWeapon = 0;
    readonly bool[] _unlockedWeapons = new bool[WEAPON_COUNT];

    int _playerLevel;
    float _playerExp, _playerExpNext, _levelUpFlash;
    int _killCount, _pickupCount;
    float _hurtFlash, _camShake;
    bool _gameOver;
    float _gameOverTimer;
    int _lastFrame = -1;
    readonly System.Random _rng = new System.Random();

    float _dashTimer = 0f;
    float _dashCd = 0f;
    float _invulnTimer = 0f;
    int _combo = 0;
    float _comboTimer = 0f;
    float _burstCd = 0f;
    float _burstFlash = 0f;
    float _timeStopTimer = 0f;
    float _timeStopCd = 0f;

    AudioSource[] _sfxPool;
    int _sfxIdx;
    float _lastShootSfxTime;

    bool _synEvaporate, _synSplit, _synThorns;
    float _thornsTimer;
    float _portalCd = 0f;

    public override void OnEnter()
    {
        var rootGO = new GameObject("VirusRoot");
        _root = rootGO.transform;
        SetupCamera();
        SetupHUD();
        InitSfx();
        ResetGame(true);
    }

    public override void Dispose()
    {
        ClearAll();
        if (_hud != null && _hud.gameObject != null) UnityEngine.Object.Destroy(_hud.gameObject);
        if (_root != null) UnityEngine.Object.Destroy(_root.gameObject);
        base.Dispose();
    }

    void InitNewGame()
    {
        _level = 0;
        _playerLevel = 1;
        _playerExp = 0f;
        _playerExpNext = ExpNext(1);
        _playerSides = 3;
        _playerRadius = PLAYER_BASE_RADIUS;
        _playerMaxHp = PLAYER_BASE_HP;
        _playerSpeed = PLAYER_BASE_SPEED;
        _playerDamage = PLAYER_BASE_DMG;
        _playerFireRate = PLAYER_BASE_FIRE_RATE;
        _bulletSpeed = PLAYER_BULLET_SPEED_INIT;
        _attractRadius = BASE_ATTRACT_RADIUS;
        _bounceCount = 0;
        _damageReduce = 0f;
        _hpRegen = 0f;
        _fireLevel = 0;
        _iceLevel = 0;
        _playerAttackRange = PLAYER_BASE_RANGE;
        _playerKnockback = 0f;
        for (int i = 0; i < WEAPON_COUNT; i++) _unlockedWeapons[i] = false;
        _unlockedWeapons[0] = true;
        _currentWeapon = 0;
    }

    void ClearAll()
    {
        for (int i = _arrows.Count - 1; i >= 0; i--)
            if (_arrows[i]?.go != null) UnityEngine.Object.Destroy(_arrows[i].go);
        _arrows.Clear();
        for (int i = _enemies.Count - 1; i >= 0; i--)
            if (_enemies[i]?.go != null) UnityEngine.Object.Destroy(_enemies[i].go);
        _enemies.Clear();
        for (int i = _pickups.Count - 1; i >= 0; i--)
            if (_pickups[i]?.go != null) UnityEngine.Object.Destroy(_pickups[i].go);
        _pickups.Clear();
        for (int i = _mapObjects.Count - 1; i >= 0; i--)
            if (_mapObjects[i]?.go != null) UnityEngine.Object.Destroy(_mapObjects[i].go);
        _mapObjects.Clear();
        if (_player?.go != null) UnityEngine.Object.Destroy(_player.go);
        _player = null;
        _boss = null;
    }

    void SetupCamera()
    {
        RefreshWorldSize();
        var targetGO = new GameObject("VirusCamTarget");
        targetGO.transform.SetParent(_root, false);
        targetGO.transform.localPosition = Vector3.zero;
        _camTarget = targetGO.transform;
    }

    void RefreshWorldSize()
    {
        _lastScreenW = Screen.width;
        _lastScreenH = Screen.height;
        _worldW = WORLD_H * ((float)_lastScreenW / Mathf.Max(1, _lastScreenH));
    }

    void TryBindCinemachine()
    {
        if (_vcam != null) return;
        var cam = Camera.main;
        if (cam == null) return;
        var brain = cam.GetComponent<CinemachineBrain>();
        if (brain == null) return;
        _vcam = brain.ActiveVirtualCamera as CinemachineCamera;
        if (_vcam == null) return;
        _vcam.Follow = _camTarget;
        var lens = _vcam.Lens;
        lens.OrthographicSize = WORLD_H * 0.5f;
        _vcam.Lens = lens;
    }

    void SetupHUD()
    {
        var go = new GameObject("VirusHUD");
        _hud = go.AddComponent<VirusHUD>();
        _hud.scene = this;
    }

    void InitSfx()
    {
        var go = new GameObject("Sfx");
        go.transform.SetParent(_root, false);
        _sfxPool = new AudioSource[4];
        for (int i = 0; i < _sfxPool.Length; i++)
        {
            var s = go.AddComponent<AudioSource>();
            s.playOnAwake = false;
            s.spatialBlend = 0f;
            s.volume = 1f;
            _sfxPool[i] = s;
        }
    }

    void PlaySfx(AudioClip clip, float volume, float pitch)
    {
        if (clip == null || _sfxPool == null) return;
        var src = _sfxPool[_sfxIdx];
        _sfxIdx = (_sfxIdx + 1) % _sfxPool.Length;
        src.pitch = pitch;
        src.PlayOneShot(clip, volume);
    }

    [Timer(0, -1)]
    void update()
    {
        if (_lastFrame == Time.frameCount) return;
        _lastFrame = Time.frameCount;

        if (Screen.width != _lastScreenW || Screen.height != _lastScreenH)
            RefreshWorldSize();

        float dt = Time.deltaTime;
        if (dt > 0.05f) dt = 0.05f;

        TryBindCinemachine();
        UpdateCamera(dt);

        _synEvaporate = _fireLevel >= SYNERGY_FIRE_LEVEL && _iceLevel >= SYNERGY_ICE_LEVEL;
        _synSplit = _bounceCount >= SYNERGY_BOUNCE && _playerFireRate >= SYNERGY_FIRE_RATE;
        _synThorns = _damageReduce >= SYNERGY_DR && _hpRegen >= SYNERGY_REGEN;

        if (_gameOver)
        {
            _gameOverTimer -= dt;
            _hurtFlash = Mathf.Max(0f, _hurtFlash - dt * 0.6f);
            if (_gameOverTimer <= 0f) ResetGame(false);
            return;
        }

        _hurtFlash = Mathf.Max(0f, _hurtFlash - dt * 3f);
        _levelUpFlash = Mathf.Max(0f, _levelUpFlash - dt * 2.5f);
        _burstFlash = Mathf.Max(0f, _burstFlash - dt * 2f);

        _dashTimer = Mathf.Max(0f, _dashTimer - dt);
        _dashCd = Mathf.Max(0f, _dashCd - dt);
        _invulnTimer = Mathf.Max(0f, _invulnTimer - dt);
        _burstCd = Mathf.Max(0f, _burstCd - dt);
        _timeStopCd = Mathf.Max(0f, _timeStopCd - dt);
        _portalCd = Mathf.Max(0f, _portalCd - dt);

        if (_timeStopTimer > 0f) _timeStopTimer -= dt;

        if (_combo > 0)
        {
            _comboTimer -= dt;
            if (_comboTimer <= 0f) { _combo = 0; _comboTimer = 0f; }
        }

        HandleSkillInput();
        HandleWeaponInput();

        UpdatePlayer(dt);
        UpdateMapObjects(dt);
        UpdateSpawning(dt);
        UpdateEnemies(dt);
        UpdateArrows(dt);
        UpdatePickups(dt);
    }

    void HandleSkillInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.eKey.wasPressedThisFrame && _burstCd <= 0f) { DoBurst(); _burstCd = BURST_CD; }
        if (kb.qKey.wasPressedThisFrame && _timeStopCd <= 0f)
        {
            _timeStopTimer = TIMESTOP_DURATION;
            _timeStopCd = TIMESTOP_CD;
            PlaySfx(SfxGen.LevelUp(), 0.5f, 0.5f);
            _camShake = Mathf.Max(_camShake, 0.3f);
        }
    }

    void HandleWeaponInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.digit1Key.wasPressedThisFrame && _unlockedWeapons[0]) SwitchWeapon(0);
        if (kb.digit2Key.wasPressedThisFrame && _unlockedWeapons[1]) SwitchWeapon(1);
        if (kb.digit3Key.wasPressedThisFrame && _unlockedWeapons[2]) SwitchWeapon(2);
        if (kb.digit4Key.wasPressedThisFrame && _unlockedWeapons[3]) SwitchWeapon(3);
    }

    void SwitchWeapon(int w)
    {
        if (_currentWeapon == w) return;
        _currentWeapon = w;
        PlaySfx(SfxGen.Pickup(), 0.4f, 1.2f);
        if (_player != null)
        {
            Color c = WeaponColor(w);
            _player.tr.GetComponent<MeshRenderer>().sharedMaterial = VMesh.Mat(c);
        }
    }

    Color WeaponColor(int w)
    {
        switch (w)
        {
            case 1: return new Color(1f, 0.7f, 0.3f);
            case 2: return new Color(0.7f, 0.9f, 1f);
            case 3: return new Color(1f, 0.4f, 0.4f);
            default: return new Color(0.30f, 0.92f, 1f);
        }
    }

    string WeaponName(int w)
    {
        switch (w)
        {
            case 1: return "散射枪";
            case 2: return "回旋镖";
            case 3: return "爆裂箭";
            default: return "多重箭";
        }
    }

    void DoBurst()
    {
        if (_player == null || !_player.alive) return;
        _burstFlash = 1f;
        _camShake = Mathf.Max(_camShake, 0.7f);
        PlaySfx(SfxGen.Kill(), 0.6f, 0.8f);

        Vector2 center = _player.pos;
        float r2 = BURST_RADIUS * BURST_RADIUS;
        _killBuffer.Clear();
        for (int i = 0; i < _enemies.Count; i++)
        {
            var e = _enemies[i];
            if (e == null || !e.alive) continue;
            Vector2 d = e.pos - center;
            float d2 = d.sqrMagnitude;
            if (d2 > r2) continue;
            e.hp -= BURST_DMG;
            if (d2 > 0.0001f)
            {
                float dist = Mathf.Sqrt(d2);
                Vector2 pushDir = d / dist;
                e.pos += pushDir * BURST_PUSH;
                float hw = _worldW * 0.5f - e.radius;
                float hh = WORLD_H * 0.5f - e.radius;
                e.pos.x = Mathf.Clamp(e.pos.x, -hw, hw);
                e.pos.y = Mathf.Clamp(e.pos.y, -hh, hh);
            }
            if (e.hp <= 0f) _killBuffer.Add(e);
        }
        for (int k = 0; k < _killBuffer.Count; k++)
        {
            var e = _killBuffer[k];
            if (e != null && e.alive) KillEnemy(e);
        }
        _killBuffer.Clear();
    }

    void ResetGame(bool newGame)
    {
        ClearAll();

        _killCount = 0;
        _pickupCount = 0;
        _levelUpFlash = 0f;
        _hurtFlash = 0f;
        _camShake = 0f;
        _dashTimer = 0f;
        _dashCd = 0f;
        _invulnTimer = 0f;
        _burstCd = 0f;
        _burstFlash = 0f;
        _timeStopTimer = 0f;
        _timeStopCd = 0f;
        _combo = 0;
        _comboTimer = 0f;
        _portalCd = 0f;
        _isBossLevel = false;
        _boss = null;
        _bossPhase = 0;
        _currentMod = LevelMod.None;
        _gameOver = false;

        if (newGame) InitNewGame();

        _player = CreateUnit(_playerSides, _playerRadius, WeaponColor(_currentWeapon), "Player");
        _player.isPlayer = true;
        _player.kind = EnemyKind.Normal;
        _player.pos = Vector2.zero;
        _player.dir = Vector2.up;
        _player.hp = _playerMaxHp;
        _player.maxHp = _playerMaxHp;
        _playerHp = _playerMaxHp;
        _player.fireCd = 0f;
        _player.tr.localPosition = Vector3.zero;

        _skipNextLevelIncrement = _level > 0;

        _inLevelBreak = true;
        _levelBreakTimer = 1.6f;
        _enemiesToSpawn = 0;
    }

    float ExpNext(int lv) { return 50f + (lv - 1) * 40f; }

    float CurrentBurnDps()
    {
        if (_fireLevel <= 0) return 0f;
        return BURN_DPS_BASE + (_fireLevel - 1) * BURN_DPS_PER_LEVEL;
    }

    float CurrentSlowFactor()
    {
        if (_iceLevel <= 0) return 1f;
        return Mathf.Max(SLOW_FACTOR_MIN, SLOW_FACTOR_BASE - (_iceLevel - 1) * SLOW_FACTOR_PER_LEVEL);
    }

    float ComboMul()
    {
        if (_combo <= 1) return 1f;
        return Mathf.Min(COMBO_MAX_MUL, 1f + (_combo - 1) * COMBO_PER_STACK);
    }

    int CurrentLevelMinSides()
    {
        return Mathf.Clamp(3 + _level / 2, 3, MAX_ENEMY_SIDES);
    }

    static float PointToSegmentDist(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float abLenSq = ab.sqrMagnitude;
        if (abLenSq < 1e-8f) return (p - a).magnitude;
        float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / abLenSq);
        Vector2 proj = a + t * ab;
        return (p - proj).magnitude;
    }

    // =====================================================================
    //  地图
    // =====================================================================
    void SpawnMapObjects(int lv)
    {
        for (int i = _mapObjects.Count - 1; i >= 0; i--)
            if (_mapObjects[i]?.go != null) UnityEngine.Object.Destroy(_mapObjects[i].go);
        _mapObjects.Clear();

        float hw = _worldW * 0.5f;
        float hh = WORLD_H * 0.5f;

        int obstacleCount = 3 + _rng.Next(4);
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector2 p = RandomMapPos(4f, hw, hh);
            CreateMapObject(MapKind.Obstacle, p, OBSTACLE_RADIUS);
        }

        if (lv >= 3)
        {
            int n = 1 + _rng.Next(2);
            for (int i = 0; i < n; i++)
            {
                Vector2 p = RandomMapPos(3f, hw, hh);
                CreateMapObject(MapKind.EnergyPillar, p, PILLAR_RADIUS);
            }
        }

        if (lv >= 3 && _rng.Next(100) < 60)
        {
            Vector2 p1 = RandomMapPos(5f, hw, hh);
            Vector2 p2 = RandomMapPos(5f, hw, hh);
            int idx1 = _mapObjects.Count;
            int idx2 = idx1 + 1;
            var m1 = CreateMapObject(MapKind.Portal, p1, PORTAL_RADIUS);
            var m2 = CreateMapObject(MapKind.Portal, p2, PORTAL_RADIUS);
            m1.pairIndex = idx2;
            m2.pairIndex = idx1;
        }

        if (lv >= 4)
        {
            int mineCount = 3 + _rng.Next(4);
            for (int i = 0; i < mineCount; i++)
            {
                Vector2 p = RandomMapPos(3f, hw, hh);
                CreateMapObject(MapKind.Mine, p, MINE_RADIUS);
            }
        }
    }

    Vector2 RandomMapPos(float minPlayerDist, float hw, float hh)
    {
        for (int k = 0; k < 10; k++)
        {
            Vector2 p = new Vector2(
                (NextFloat() * 2f - 1f) * (hw - 1.5f),
                (NextFloat() * 2f - 1f) * (hh - 1.5f));
            if (p.magnitude >= minPlayerDist) return p;
        }
        return new Vector2(NextFloat() * 4f - 2f, NextFloat() * 4f - 2f + 4f);
    }

    MapObject CreateMapObject(MapKind kind, Vector2 pos, float radius)
    {
        var go = new GameObject("Map_" + kind);
        go.transform.SetParent(_root, false);
        go.transform.localPosition = new Vector3(pos.x, pos.y, 0.05f);

        int sides;
        Color col;
        float scale;
        switch (kind)
        {
            case MapKind.Obstacle:
                sides = 4; col = new Color(0.4f, 0.4f, 0.5f); scale = radius; break;
            case MapKind.Portal:
                sides = 8; col = new Color(0.7f, 0.4f, 1f); scale = radius * 0.8f; break;
            case MapKind.EnergyPillar:
                sides = 6; col = new Color(0.3f, 1f, 0.5f); scale = radius * 0.9f; break;
            default:
                sides = 3; col = new Color(1f, 0.5f, 0.2f); scale = radius * 0.7f; break;
        }

        go.transform.localScale = Vector3.one * scale;
        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = VMesh.Poly(sides);
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = VMesh.Mat(col);
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;

        var m = new MapObject();
        m.go = go; m.tr = go.transform; m.pos = pos; m.kind = kind;
        m.radius = radius; m.baseScale = scale; m.isAlive = true; m.age = 0f; m.color = col;
        _mapObjects.Add(m);
        return m;
    }

    void UpdateMapObjects(float dt)
    {
        for (int i = 0; i < _mapObjects.Count; i++)
        {
            var m = _mapObjects[i];
            if (m == null || !m.isAlive || m.go == null) continue;
            m.age += dt;
            switch (m.kind)
            {
                case MapKind.Portal:
                    m.tr.localRotation = Quaternion.Euler(0f, 0f, m.age * 120f);
                    {
                        float pulse = m.baseScale * (1f + Mathf.Sin(m.age * 4f) * 0.15f);
                        m.tr.localScale = new Vector3(pulse, pulse, 1f);
                    }
                    break;
                case MapKind.EnergyPillar:
                    {
                        float pulse = m.baseScale * (1f + Mathf.Sin(m.age * 3f) * 0.1f);
                        m.tr.localScale = new Vector3(pulse, pulse, 1f);
                    }
                    break;
                case MapKind.Mine:
                    m.tr.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(m.age * 2f) * 15f);
                    break;
            }
        }
    }

    void CheckPlayerMapInteraction(float dt)
    {
        if (_player == null || !_player.alive) return;

        for (int i = 0; i < _mapObjects.Count; i++)
        {
            var m = _mapObjects[i];
            if (m == null || !m.isAlive || m.kind != MapKind.Obstacle) continue;
            Vector2 d = _player.pos - m.pos;
            float dist = d.magnitude;
            float minDist = m.radius + _player.radius;
            if (dist < minDist && dist > 0.0001f)
                _player.pos = m.pos + d / dist * minDist;
        }

        if (_portalCd <= 0f)
        {
            for (int i = 0; i < _mapObjects.Count; i++)
            {
                var m = _mapObjects[i];
                if (m == null || !m.isAlive || m.kind != MapKind.Portal) continue;
                float pr = m.radius + _player.radius * 0.5f;
                if ((_player.pos - m.pos).sqrMagnitude < pr * pr)
                {
                    if (m.pairIndex >= 0 && m.pairIndex < _mapObjects.Count)
                    {
                        var pair = _mapObjects[m.pairIndex];
                        if (pair != null && pair.isAlive)
                        {
                            Vector2 dir = m.pos - pair.pos;
                            if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
                            _player.pos = pair.pos + dir.normalized * (pair.radius + _player.radius + 0.2f);
                            _portalCd = 0.8f;
                            PlaySfx(SfxGen.Dash(), 0.4f, 0.7f);
                            _camShake = Mathf.Max(_camShake, 0.25f);
                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < _mapObjects.Count; i++)
        {
            var m = _mapObjects[i];
            if (m == null || !m.isAlive || m.kind != MapKind.EnergyPillar) continue;
            float pr = m.radius;
            if ((_player.pos - m.pos).sqrMagnitude < pr * pr)
            {
                if (_playerHp < _playerMaxHp && _currentMod != LevelMod.BloodMoon)
                {
                    _playerHp = Mathf.Min(_playerMaxHp, _playerHp + PILLAR_HEAL_RATE * dt);
                    _player.hp = _playerHp;
                }
            }
        }

        for (int i = 0; i < _mapObjects.Count; i++)
        {
            var m = _mapObjects[i];
            if (m == null || !m.isAlive || m.kind != MapKind.Mine) continue;
            float pr = m.radius + _player.radius;
            if ((_player.pos - m.pos).sqrMagnitude < pr * pr) { TriggerMine(m); break; }
        }
    }

    void TriggerMine(MapObject m)
    {
        m.isAlive = false;
        if (m.go != null) UnityEngine.Object.Destroy(m.go);
        PlaySfx(SfxGen.Kill(), 0.5f, 0.9f);
        _camShake = Mathf.Max(_camShake, 0.5f);

        float r2 = MINE_BLAST_RADIUS * MINE_BLAST_RADIUS;
        if (_invulnTimer <= 0f && (_player.pos - m.pos).sqrMagnitude < r2)
            DamagePlayer(MINE_DMG);

        _killBuffer.Clear();
        for (int i = 0; i < _enemies.Count; i++)
        {
            var e = _enemies[i];
            if (e == null || !e.alive) continue;
            if ((e.pos - m.pos).sqrMagnitude < r2)
            {
                e.hp -= MINE_DMG;
                if (e.hp <= 0f) _killBuffer.Add(e);
            }
        }
        for (int k = 0; k < _killBuffer.Count; k++)
        {
            var e = _killBuffer[k];
            if (e != null && e.alive) KillEnemy(e);
        }
        _killBuffer.Clear();
    }

    bool LineHitsObstacle(Vector2 from, Vector2 to, out MapObject hit)
    {
        hit = null;
        for (int i = 0; i < _mapObjects.Count; i++)
        {
            var m = _mapObjects[i];
            if (m == null || !m.isAlive || m.kind != MapKind.Obstacle) continue;
            if (PointToSegmentDist(m.pos, from, to) < m.radius)
            {
                hit = m;
                return true;
            }
        }
        return false;
    }

    // =====================================================================
    //  关卡
    // =====================================================================
    LevelMod RollLevelMod(int lv)
    {
        if (lv <= 2) return LevelMod.None;
        if (lv % BOSS_EVERY == 0) return LevelMod.None;
        if (_rng.Next(100) < 20) return LevelMod.None;
        int r = _rng.Next(5);
        switch (r)
        {
            case 0: return LevelMod.Swarm;
            case 1: return LevelMod.Hardened;
            case 2: return LevelMod.Swift;
            case 3: return LevelMod.GlassCannon;
            default: return LevelMod.BloodMoon;
        }
    }

    string LevelModName(LevelMod m)
    {
        switch (m)
        {
            case LevelMod.Swarm: return "🐝 蜂群";
            case LevelMod.Hardened: return "🛡 硬化";
            case LevelMod.Swift: return "⚡ 极速";
            case LevelMod.GlassCannon: return "💥 玻璃炮";
            case LevelMod.BloodMoon: return "🩸 血月";
        }
        return "";
    }

    void StartLevel(int lv)
    {
        _level = lv;
        _playerHp = _playerMaxHp;
        if (_player != null) _player.hp = _playerHp;

        _currentMod = RollLevelMod(lv);
        SpawnMapObjects(lv);

        if (lv % BOSS_EVERY == 0)
        {
            _isBossLevel = true;
            _enemiesToSpawn = 0;
            SpawnBoss();
        }
        else
        {
            _isBossLevel = false;
            _enemiesToSpawn = 5 + lv * 2;
            if (_currentMod == LevelMod.Swarm) _enemiesToSpawn *= 2;
        }
        _spawnTimer = 0.3f;
        _inLevelBreak = false;
    }

    void UpdateSpawning(float dt)
    {
        if (_inLevelBreak)
        {
            _levelBreakTimer -= dt;
            if (_levelBreakTimer <= 0f)
            {
                if (_skipNextLevelIncrement) { _skipNextLevelIncrement = false; StartLevel(_level); }
                else StartLevel(_level + 1);
            }
            return;
        }

        if (_isBossLevel)
        {
            if (_enemies.Count == 0) { _inLevelBreak = true; _levelBreakTimer = 2.0f; }
            return;
        }

        if (_enemiesToSpawn > 0)
        {
            int maxAlive = 6 + _level * 2;
            if (_currentMod == LevelMod.Swarm) maxAlive = Mathf.RoundToInt(maxAlive * 1.5f);

            if (_enemies.Count < maxAlive)
            {
                _spawnTimer -= dt;
                if (_spawnTimer <= 0f)
                {
                    SpawnEnemy(PickEnemyKind());
                    _enemiesToSpawn--;
                    _spawnTimer = Mathf.Max(0.25f, 1.2f - _level * 0.05f);
                }
            }
        }
        else if (_enemies.Count == 0)
        {
            _inLevelBreak = true;
            _levelBreakTimer = 2.0f;
        }
    }

    EnemyKind PickEnemyKind()
    {
        int lv = _level;
        int normalW = 55;
        int chargerW = lv >= 2 ? 15 + lv : 0;
        int shooterW = lv >= 2 ? 15 + lv : 0;
        int tankW = lv >= 3 ? 12 + lv : 0;
        int total = normalW + chargerW + shooterW + tankW;
        int r = _rng.Next(total);
        if (r < normalW) return EnemyKind.Normal;
        r -= normalW;
        if (r < chargerW) return EnemyKind.Charger;
        r -= chargerW;
        if (r < shooterW) return EnemyKind.Shooter;
        return EnemyKind.Tank;
    }

    void SpawnBoss()
    {
        int lv = _level;
        float hp = 300f + lv * 150f;
        var b = CreateUnit(BOSS_SIDES, BOSS_RADIUS, new Color(1f, 0.2f, 0.4f), "Boss");
        b.kind = EnemyKind.Boss;
        b.isBoss = true;
        b.pos = new Vector2(0f, WORLD_H * 0.3f);
        b.hp = hp; b.maxHp = hp;
        b.speed = 2.5f;
        b.damage = 25f + lv * 2f;
        b.isPlayer = false;
        b.dir = Vector2.down;
        b.fireCd = 0f; b.canShoot = true;
        b.shootCd = 0.5f; b.shootInterval = 1.6f; b.aiTimer = 2.5f;
        b.tr.localPosition = new Vector3(b.pos.x, b.pos.y, 0f);
        b.fireFx = CreateStatusFx(b.tr, true);
        b.iceFx = CreateStatusFx(b.tr, false);
        _enemies.Add(b);
        _boss = b;
        _bossPhase = 1;
        PlaySfx(SfxGen.LevelUp(), 0.7f, 0.5f);
        _camShake = 0.6f;
    }

    void UpdateBoss(Unit b, float dt)
    {
        float ratio = b.maxHp > 0f ? b.hp / b.maxHp : 0f;
        int newPhase = ratio > 0.66f ? 1 : (ratio > 0.33f ? 2 : 3);
        if (newPhase != _bossPhase)
        {
            _bossPhase = newPhase;
            _camShake = Mathf.Max(_camShake, 0.6f);
            PlaySfx(SfxGen.LevelUp(), 0.6f, 0.6f);
        }

        Vector2 to = _player.pos - b.pos;
        float dist = to.magnitude;
        Vector2 dir = dist > 0.0001f ? to / dist : Vector2.up;
        b.dir = dir;

        float speedMul = _bossPhase == 3 ? 1.5f : 1f;
        if (b.slowTime > 0f) speedMul *= b.slowFactor;
        if (_timeStopTimer > 0f) speedMul *= TIMESTOP_SLOW;

        float keep = 6f;
        if (dist > keep) b.pos += dir * b.speed * speedMul * dt;
        else if (dist < keep - 1f) b.pos -= dir * b.speed * 0.5f * speedMul * dt;

        float hw = _worldW * 0.5f - b.radius;
        float hh = WORLD_H * 0.5f - b.radius;
        b.pos.x = Mathf.Clamp(b.pos.x, -hw, hw);
        b.pos.y = Mathf.Clamp(b.pos.y, -hh, hh);

        b.shootCd -= dt;

        if (_bossPhase == 1)
        {
            if (b.shootCd <= 0f) { b.shootCd = 1.6f; FireBossRing(b, 16); }
        }
        else if (_bossPhase == 2)
        {
            if (b.shootCd <= 0f) { b.shootCd = 1.4f; FireBossRing(b, 20); }
            b.aiTimer -= dt;
            if (b.aiTimer <= 0f)
            {
                b.aiTimer = 4.5f;
                for (int k = 0; k < 3; k++)
                {
                    if (_enemies.Count > 25) break;
                    float ang = NextFloat() * Mathf.PI * 2f;
                    Vector2 p = b.pos + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * 3f;
                    SpawnEnemyAt(PickEnemyKind(), p);
                }
            }
        }
        else
        {
            if (b.shootCd <= 0f) { b.shootCd = 0.8f; FireBossRing(b, 24); }
            b.aiTimer -= dt;
            if (b.aiTimer <= 0f)
            {
                b.aiTimer = 3.0f;
                if (_enemies.Count < 30)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        float ang = NextFloat() * Mathf.PI * 2f;
                        Vector2 p = b.pos + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * 3f;
                        SpawnEnemyAt(PickEnemyKind(), p);
                    }
                }
            }
        }

        float stop = b.radius + _player.radius + 0.1f;
        if (dist <= stop)
        {
            b.fireCd -= dt;
            if (b.fireCd <= 0f) { b.fireCd = 0.75f; DamagePlayer(b.damage); }
        }
    }

    void FireBossRing(Unit b, int count)
    {
        float speed = ENEMY_BULLET_SPEED;
        float dmg = b.damage * 0.45f;
        float size = b.radius * 0.22f;
        Color col = new Color(1f, 0.4f, 0.6f);
        float baseAngle = NextFloat() * Mathf.PI * 2f;
        for (int i = 0; i < count; i++)
        {
            float a = baseAngle + i * Mathf.PI * 2f / count;
            Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Vector2 start = b.pos + dir * (b.radius + 0.3f);
            var arrow = CreateArrow(start, dir, size, dmg, speed, false, col);
            arrow.originPos = b.pos;
            arrow.maxRange = ENEMY_BULLET_RANGE;
            _arrows.Add(arrow);
        }
        PlaySfx(SfxGen.EnemyShoot(), 0.35f, 0.7f);
    }

    void SpawnEnemy(EnemyKind kind) { SpawnEnemyAt(kind, PickSpawnPos(1f)); }

    Vector2 PickSpawnPos(float radiusHint)
    {
        float ang = NextFloat() * Mathf.PI * 2f;
        float hw = _worldW * 0.5f;
        float hh = WORLD_H * 0.5f;
        float cx = Mathf.Cos(ang);
        float cy = Mathf.Sin(ang);
        float tx = Mathf.Abs(cx) > 0.0001f ? hw / Mathf.Abs(cx) : float.MaxValue;
        float ty = Mathf.Abs(cy) > 0.0001f ? hh / Mathf.Abs(cy) : float.MaxValue;
        float edgeDist = Mathf.Min(tx, ty);
        float spawnDist = edgeDist + radiusHint + SPAWN_MARGIN;
        return new Vector2(cx * spawnDist, cy * spawnDist);
    }

    void SpawnEnemyAt(EnemyKind kind, Vector2 pos)
    {
        int lv = _level;
        int minSides, maxSides;
        float radius, hp, speed, damage;
        Color col;

        switch (kind)
        {
            case EnemyKind.Charger:
                minSides = 3; maxSides = 7; radius = 0.42f;
                hp = 18f + lv * 14f; speed = 3.5f + lv * 0.22f;
                damage = 10f + lv * 2.0f; col = new Color(1f, 0.35f, 0.25f);
                break;
            case EnemyKind.Shooter:
                minSides = 4; maxSides = 8; radius = 0.55f;
                hp = 24f + lv * 20f; speed = 2.8f + lv * 0.18f;
                damage = 5f + lv * 1.1f; col = new Color(0.3f, 0.85f, 1f);
                break;
            case EnemyKind.Tank:
                minSides = 5; maxSides = 9; radius = 0.9f;
                hp = 70f + lv * 45f; speed = 2.0f + lv * 0.12f;
                damage = 14f + lv * 2.2f; col = new Color(0.75f, 0.55f, 0.95f);
                break;
            default:
                minSides = Mathf.Min(MAX_ENEMY_SIDES, 3 + lv / 2);
                maxSides = Mathf.Min(MAX_ENEMY_SIDES, 4 + lv);
                radius = 0.4f;
                hp = 20f + lv * 18f; speed = 3.8f + lv * 0.25f;
                damage = 7f + lv * 1.6f; col = EnemyColor(minSides);
                break;
        }

        if (maxSides < minSides) maxSides = minSides;
        if (maxSides < 3) maxSides = 3;
        int span = maxSides - minSides + 1;
        if (span < 1) span = 1;
        int sides = minSides + _rng.Next(span);

        if (kind == EnemyKind.Normal)
        {
            radius = 0.4f + sides * 0.035f;
            hp += (sides - 3) * 12f;
            damage += (sides - 3) * 0.8f;
        }
        else hp += (sides - minSides) * 6f;

        switch (_currentMod)
        {
            case LevelMod.Swarm: hp *= 0.6f; break;
            case LevelMod.Hardened: hp *= 1.5f; speed *= 0.7f; break;
            case LevelMod.Swift: speed *= 1.4f; damage *= 0.7f; break;
        }

        var e = CreateUnit(sides, radius, col, "Enemy");
        e.kind = kind; e.pos = pos;
        e.hp = hp; e.maxHp = hp;
        e.speed = speed; e.damage = damage;
        e.isPlayer = false; e.dir = Vector2.zero;
        e.fireCd = 0f; e.canShoot = true; e.isSplit = false;

        switch (kind)
        {
            case EnemyKind.Charger:
                e.shootInterval = 999f;
                e.aiTimer = 0.9f + NextFloat() * 0.7f; break;
            case EnemyKind.Shooter:
                e.shootInterval = Mathf.Max(0.7f, 2.0f - lv * 0.07f);
                e.shootCd = 0.4f + NextFloat() * 0.7f; break;
            case EnemyKind.Tank:
                e.shootInterval = 999f; break;
            default:
                e.shootInterval = Mathf.Max(0.9f, 2.4f - lv * 0.10f);
                e.shootCd = 0.4f + NextFloat() * 0.7f; break;
        }

        e.tr.localPosition = new Vector3(e.pos.x, e.pos.y, 0f);
        e.fireFx = CreateStatusFx(e.tr, true);
        e.iceFx = CreateStatusFx(e.tr, false);
        _enemies.Add(e);
    }

    void SpawnSplitEnemy(Vector2 pos, int sides)
    {
        int lv = _level;
        float radius = 0.35f + sides * 0.035f;
        float hp = 10f + lv * 10f + (sides - 3) * 8f;
        float speed = 4.0f + lv * 0.25f;
        float damage = 4f + lv * 1.0f + (sides - 3) * 0.5f;

        switch (_currentMod)
        {
            case LevelMod.Swarm: hp *= 0.6f; break;
            case LevelMod.Hardened: hp *= 1.5f; speed *= 0.7f; break;
            case LevelMod.Swift: speed *= 1.4f; damage *= 0.7f; break;
        }

        var e = CreateUnit(sides, radius, EnemyColor(sides), "Enemy");
        e.kind = EnemyKind.Normal; e.pos = pos;
        e.hp = hp; e.maxHp = hp;
        e.speed = speed; e.damage = damage;
        e.isPlayer = false; e.dir = Vector2.zero;
        e.fireCd = 0f; e.canShoot = true;
        e.shootInterval = Mathf.Max(1.0f, 2.6f - lv * 0.08f);
        e.shootCd = 0.6f + NextFloat() * 1.0f;
        e.isSplit = true;
        e.tr.localPosition = new Vector3(e.pos.x, e.pos.y, 0f);
        e.fireFx = CreateStatusFx(e.tr, true);
        e.iceFx = CreateStatusFx(e.tr, false);
        _enemies.Add(e);
    }

    Color EnemyColor(int sides)
    {
        float h = Mathf.Repeat(0.02f + (sides - 3) * 0.075f, 1f);
        return Color.HSVToRGB(h, 0.78f, 1f);
    }

    ParticleSystem CreateStatusFx(Transform parent, bool isFire)
    {
        var go = new GameObject(isFire ? "FireFx" : "IceFx");
        go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1f; main.loop = true;
        main.startLifetime = isFire ? 0.55f : 0.7f;
        main.startSpeed = isFire ? 2.0f : 1.0f;
        main.startSize = isFire ? 0.45f : 0.4f;
        main.startColor = isFire ? new Color(1f, 0.7f, 0.3f) : new Color(0.55f, 0.85f, 1f);
        main.gravityModifier = isFire ? -0.5f : 0f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 60;
        main.playOnAwake = false;

        var emission = ps.emission;
        emission.rateOverTime = isFire ? 40f : 30f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.9f; shape.radiusThickness = 1f;

        var colLife = ps.colorOverLifetime;
        colLife.enabled = true;
        Gradient grad = new Gradient();
        if (isFire)
        {
            grad.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(1f, 0.98f, 0.75f), 0f),
                    new GradientColorKey(new Color(1f, 0.55f, 0.15f), 0.45f),
                    new GradientColorKey(new Color(0.55f, 0.12f, 0.03f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.15f),
                    new GradientAlphaKey(0.7f, 0.55f), new GradientAlphaKey(0f, 1f)
                });
        }
        else
        {
            grad.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(0.95f, 1f, 1f), 0f),
                    new GradientColorKey(new Color(0.55f, 0.85f, 1f), 0.45f),
                    new GradientColorKey(new Color(0.15f, 0.4f, 0.85f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.15f),
                    new GradientAlphaKey(0.7f, 0.55f), new GradientAlphaKey(0f, 1f)
                });
        }
        colLife.color = new ParticleSystem.MinMaxGradient(grad);

        var sizeLife = ps.sizeOverLifetime;
        sizeLife.enabled = true;
        sizeLife.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0f, 1f, 1f, 0.3f));

        var velLife = ps.velocityOverLifetime;
        velLife.enabled = true;
        velLife.space = ParticleSystemSimulationSpace.Local;
        if (isFire)
        {
            velLife.x = new ParticleSystem.MinMaxCurve(0f, 0f);
            velLife.y = new ParticleSystem.MinMaxCurve(1.2f, 1.2f);
            velLife.z = new ParticleSystem.MinMaxCurve(0f, 0f);
        }
        else
        {
            velLife.x = new ParticleSystem.MinMaxCurve(-0.4f, 0.4f);
            velLife.y = new ParticleSystem.MinMaxCurve(-0.4f, 0.4f);
            velLife.z = new ParticleSystem.MinMaxCurve(0f, 0f);
        }

        var rotLife = ps.rotationOverLifetime;
        rotLife.enabled = true;
        float rotSpeed = isFire ? 90f : -180f;
        rotLife.z = new ParticleSystem.MinMaxCurve(-rotSpeed * Mathf.Deg2Rad, rotSpeed * Mathf.Deg2Rad);

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.material = VMesh.ParticleMat();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.alignment = ParticleSystemRenderSpace.View;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.sortingOrder = 40;
        return ps;
    }

    void UpdateStatusFx(Unit e)
    {
        if (e.fireFx != null)
        {
            bool on = e.burnTime > 0f;
            if (on && !e.fireFx.isEmitting) e.fireFx.Play();
            else if (!on && e.fireFx.isEmitting) e.fireFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (e.iceFx != null)
        {
            bool on = e.slowTime > 0f;
            if (on && !e.iceFx.isEmitting) e.iceFx.Play();
            else if (!on && e.iceFx.isEmitting) e.iceFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    Vector2 _lastInput = Vector2.zero;

    void UpdatePlayer(float dt)
    {
        if (_player == null || !_player.alive) return;

        if (_hpRegen > 0f && _playerHp < _playerMaxHp && _currentMod != LevelMod.BloodMoon)
        {
            _playerHp = Mathf.Min(_playerMaxHp, _playerHp + _hpRegen * dt);
            _player.hp = _playerHp;
        }

        Vector2 input = _lastInput;
        var kb = Keyboard.current;
        if (kb != null)
        {
            input = Vector2.zero;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) input.y += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) input.y -= 1f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input.x -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x += 1f;
            if (input.sqrMagnitude > 1f) input.Normalize();
            _lastInput = input;

            if ((kb.leftShiftKey.wasPressedThisFrame || kb.rightShiftKey.wasPressedThisFrame)
                && _dashCd <= 0f && _dashTimer <= 0f)
            {
                Vector2 ddir = input.sqrMagnitude > 0.01f ? input.normalized : _player.dir;
                if (ddir.sqrMagnitude < 0.01f) ddir = Vector2.up;
                _dashTimer = DASH_DURATION;
                _dashCd = DASH_CD;
                _invulnTimer = Mathf.Max(_invulnTimer, DASH_DURATION + DASH_INVULN);
                _player.pos += ddir * 0.3f;
                PlaySfx(SfxGen.Dash(), 0.4f, 0.95f + NextFloat() * 0.1f);
            }
        }

        if (_dashTimer > 0f)
        {
            Vector2 ddir = input.sqrMagnitude > 0.01f ? input.normalized : _player.dir;
            if (ddir.sqrMagnitude < 0.01f) ddir = Vector2.up;
            _player.pos += ddir * _playerSpeed * DASH_SPEED_MUL * dt;
        }
        else
        {
            _player.pos += input * _playerSpeed * dt;
        }

        float hw = _worldW * 0.5f - _player.radius;
        float hh = WORLD_H * 0.5f - _player.radius;
        _player.pos.x = Mathf.Clamp(_player.pos.x, -hw, hw);
        _player.pos.y = Mathf.Clamp(_player.pos.y, -hh, hh);

        CheckPlayerMapInteraction(dt);

        Unit target = FindNearestEnemy(_player.pos);
        if (target != null)
        {
            Vector2 to = target.pos - _player.pos;
            if (to.sqrMagnitude > 0.0001f) _player.dir = to.normalized;
            _player.fireCd -= dt;
            if (_player.fireCd <= 0f)
            {
                _player.fireCd = 1f / Mathf.Max(0.2f, _playerFireRate);
                FireWithWeapon(_player, true);
            }
        }
        else _player.fireCd = 0f;

        float ang = Mathf.Atan2(_player.dir.y, _player.dir.x) * Mathf.Rad2Deg - 90f - 180f / Mathf.Max(3, _playerSides);
        _player.tr.localRotation = Quaternion.Euler(0f, 0f, ang);
        _player.tr.localPosition = new Vector3(_player.pos.x, _player.pos.y, 0f);
    }

    Unit FindNearestEnemy(Vector2 from)
    {
        Unit best = null;
        float bestSq = float.MaxValue;
        for (int i = 0; i < _enemies.Count; i++)
        {
            var e = _enemies[i];
            if (e == null || !e.alive) continue;
            float sq = (e.pos - from).sqrMagnitude;
            if (sq < bestSq) { bestSq = sq; best = e; }
        }
        return best;
    }

    void UpdateEnemies(float dt)
    {
        if (_synThorns && _player != null && _player.alive)
        {
            _thornsTimer -= dt;
            if (_thornsTimer <= 0f)
            {
                _thornsTimer = 0.25f;
                float dmg = SYNERGY_THORNS_DPS * 0.25f;
                _killBuffer.Clear();
                for (int i = 0; i < _enemies.Count; i++)
                {
                    var e = _enemies[i];
                    if (e == null || !e.alive) continue;
                    float rr = e.radius + _player.radius + SYNERGY_THORNS_RADIUS;
                    if ((e.pos - _player.pos).sqrMagnitude < rr * rr)
                    {
                        e.hp -= dmg;
                        if (e.hp <= 0f) _killBuffer.Add(e);
                    }
                }
                for (int k = 0; k < _killBuffer.Count; k++)
                {
                    var e = _killBuffer[k];
                    if (e != null && e.alive) KillEnemy(e);
                }
                _killBuffer.Clear();
            }
        }
        else _thornsTimer = 0f;

        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (i >= _enemies.Count) continue;
            var e = _enemies[i];
            if (e == null || !e.alive || e.go == null) { _enemies.RemoveAt(i); continue; }

            if (e.burnTime > 0f)
            {
                e.burnTime -= dt;
                e.hp -= e.burnDps * dt;
                if (e.hp <= 0f) { KillEnemy(e); continue; }
            }
            if (e.slowTime > 0f) e.slowTime -= dt;

            if (_player != null && _player.alive)
            {
                if (e.isBoss) UpdateBoss(e, dt);
                else
                {
                    float speedMul = e.slowTime > 0f ? e.slowFactor : 1f;
                    if (_timeStopTimer > 0f) speedMul *= TIMESTOP_SLOW;

                    Vector2 to = _player.pos - e.pos;
                    float dist = to.magnitude;
                    if (dist > 0.0001f)
                    {
                        Vector2 dir = to / dist;
                        switch (e.kind)
                        {
                            case EnemyKind.Charger: UpdateCharger(e, dt, dir, dist, speedMul); break;
                            case EnemyKind.Shooter: UpdateShooter(e, dt, dir, dist, speedMul); break;
                            case EnemyKind.Tank: UpdateTank(e, dt, dir, dist, speedMul); break;
                            default: UpdateNormal(e, dt, dir, dist, speedMul); break;
                        }
                    }
                }
            }

            if (!e.isBoss)
            {
                int cnt = _enemies.Count;
                for (int j = 0; j < cnt; j++)
                {
                    if (j == i) continue;
                    if (j >= _enemies.Count) break;
                    var o = _enemies[j];
                    if (o == null || !o.alive) continue;
                    Vector2 d = e.pos - o.pos;
                    float dd = d.magnitude;
                    float minD = e.radius + o.radius;
                    if (dd < minD && dd > 0.0001f)
                        e.pos += d / dd * ((minD - dd) * 0.5f);
                }

                // ★ 障碍物挡住敌人
                for (int oi = 0; oi < _mapObjects.Count; oi++)
                {
                    var mo = _mapObjects[oi];
                    if (mo == null || !mo.isAlive || mo.kind != MapKind.Obstacle) continue;
                    Vector2 dd = e.pos - mo.pos;
                    float mdist = dd.magnitude;
                    float minDist = mo.radius + e.radius;
                    if (mdist < minDist && mdist > 0.0001f)
                        e.pos = mo.pos + dd / mdist * minDist;
                }
            }

            if (i >= _enemies.Count) continue;

            if (e.dir.sqrMagnitude > 0.0001f)
            {
                float ang = Mathf.Atan2(e.dir.y, e.dir.x) * Mathf.Rad2Deg - 90f - 180f / Mathf.Max(3, e.sides);
                e.tr.localRotation = Quaternion.Euler(0f, 0f, ang);
            }
            e.tr.localPosition = new Vector3(e.pos.x, e.pos.y, 0f);

            UpdateStatusFx(e);
        }
    }

    void MeleeHit(Unit e, float dist, float stop)
    {
        if (dist <= stop)
        {
            e.fireCd -= Time.deltaTime;
            if (e.fireCd <= 0f) { e.fireCd = 0.75f; DamagePlayer(e.damage); }
        }
    }

    void UpdateNormal(Unit e, float dt, Vector2 dir, float dist, float speedMul)
    {
        e.dir = dir;
        float stop = e.radius + _player.radius + 0.06f;
        if (dist > stop) e.pos += dir * (e.speed * speedMul) * dt;
        else MeleeHit(e, dist, stop);

        if (e.canShoot && dist < 30f)
        {
            e.shootCd -= dt;
            if (e.shootCd <= 0f) { e.shootCd = e.shootInterval; FireWithWeapon(e, false); }
        }
    }

    void UpdateCharger(Unit e, float dt, Vector2 dir, float dist, float speedMul)
    {
        e.dir = dir;
        e.aiTimer -= dt;
        if (e.chargeReady)
        {
            float chargeSpeed = e.speed * 3.0f * speedMul;
            e.pos += e.chargeDir * chargeSpeed * dt;
            e.aiTimer -= dt;
            if (e.aiTimer <= 0f)
            {
                e.chargeReady = false;
                e.aiTimer = 0.9f + NextFloat() * 0.5f;
            }
        }
        else
        {
            float stop = e.radius + _player.radius + 0.06f;
            if (dist > stop) e.pos += dir * (e.speed * 0.4f * speedMul) * dt;
            else MeleeHit(e, dist, stop);

            if (e.aiTimer <= 0f)
            {
                e.chargeReady = true;
                e.chargeDir = dir;
                e.aiTimer = 0.6f;
                PlaySfx(SfxGen.Charge(), 0.25f, 1f);
            }
        }
    }

    void UpdateShooter(Unit e, float dt, Vector2 dir, float dist, float speedMul)
    {
        e.dir = dir;
        float minK = 7f, maxK = 12f;
        if (dist < minK) e.pos -= dir * (e.speed * speedMul) * dt;
        else if (dist > maxK) e.pos += dir * (e.speed * speedMul) * dt;

        if (e.canShoot && dist < 30f)
        {
            e.shootCd -= dt;
            if (e.shootCd <= 0f) { e.shootCd = e.shootInterval; FireWithWeapon(e, false); }
        }
    }

    void UpdateTank(Unit e, float dt, Vector2 dir, float dist, float speedMul)
    {
        e.dir = dir;
        float stop = e.radius + _player.radius + 0.06f;
        if (dist > stop) e.pos += dir * (e.speed * speedMul) * dt;
        else
        {
            e.fireCd -= dt;
            if (e.fireCd <= 0f) { e.fireCd = 0.7f; DamagePlayer(e.damage); }
        }
    }

    void KillEnemy(Unit e)
    {
        if (e == null || !e.alive) return;
        e.alive = false;

        Vector2 deathPos = e.pos;
        int sides = e.sides;
        bool isBoss = e.isBoss;
        bool hadBurn = e.burnTime > 0f;
        bool hadSlow = e.slowTime > 0f;

        if (e.go != null) UnityEngine.Object.Destroy(e.go);
        _enemies.Remove(e);

        if (isBoss)
        {
            _boss = null;
            _camShake = 1.2f;
            PlaySfx(SfxGen.Kill(), 0.8f, 0.6f);

            // ★ Boss 必掉一个武器碎片
            if (NextLockedWeapon() >= 0)
                SpawnPickupOfType(deathPos, PK_WEAPON);

            // 外加 5 个普通掉落
            for (int k = 0; k < 5; k++)
            {
                Vector2 off = new Vector2((NextFloat() - 0.5f) * 3f, (NextFloat() - 0.5f) * 3f);
                SpawnPickup(deathPos + off, 5);
            }
            _killCount++;
            return;
        }

        _killCount++;
        _combo++;
        _comboTimer = COMBO_TIMEOUT;
        PlaySfx(SfxGen.Kill(), 0.45f, 0.9f + NextFloat() * 0.2f);
        if (_combo >= 5) PlaySfx(SfxGen.Combo(), 0.3f, 1f + Mathf.Min(0.5f, _combo * 0.03f));

        if (_synEvaporate && hadBurn && hadSlow)
        {
            float r2 = SYNERGY_EVAPORATE_RADIUS * SYNERGY_EVAPORATE_RADIUS;
            _killBuffer.Clear();
            for (int j = 0; j < _enemies.Count; j++)
            {
                var o = _enemies[j];
                if (o == null || !o.alive) continue;
                if ((o.pos - deathPos).sqrMagnitude < r2)
                {
                    o.hp -= SYNERGY_EVAPORATE_DMG;
                    if (o.hp <= 0f) _killBuffer.Add(o);
                }
            }
            for (int k = 0; k < _killBuffer.Count; k++)
            {
                var o = _killBuffer[k];
                if (o != null && o.alive) KillEnemy(o);
            }
            _killBuffer.Clear();
            PlaySfx(SfxGen.Kill(), 0.5f, 0.7f);
            _camShake = Mathf.Max(_camShake, 0.3f);
        }

        int splitFloor = CurrentLevelMinSides();
        if (sides > splitFloor)
        {
            for (int k = 0; k < 2; k++)
            {
                Vector2 off = new Vector2((NextFloat() - 0.5f) * 1.0f, (NextFloat() - 0.5f) * 1.0f);
                SpawnSplitEnemy(deathPos + off, sides - 1);
            }
        }

        // 普通怪掉落（不再有武器碎片）
        float baseChance = Mathf.Min(DROP_CHANCE_MAX, DROP_CHANCE_BASE + _level * DROP_CHANCE_PER_LV);
        float sidesFactor = sides == 3 ? 1f : (sides == 4 ? 0.6f : 0.3f);
        float chance = baseChance * sidesFactor;
        if (_currentMod == LevelMod.BloodMoon) chance *= 2f;
        if (NextFloat() < chance) SpawnPickup(deathPos, sides);

        _camShake = Mathf.Max(_camShake, 0.12f);
    }

    void SpawnPickup(Vector2 pos, int enemySides) { SpawnPickupOfType(pos, RollPickupType()); }

    void SpawnPickupOfType(Vector2 pos, int type)
    {
        Color col = PickupColor(type);
        var go = new GameObject("Pickup_" + type);
        go.transform.SetParent(_root, false);
        go.transform.localScale = Vector3.one * 0.55f;
        go.transform.localPosition = new Vector3(pos.x, pos.y, -0.05f);

        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = VMesh.Poly(3);
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = VMesh.Mat(col);
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;

        var p = new Pickup();
        p.go = go; p.tr = go.transform; p.pos = pos; p.type = type;
        p.bobPhase = NextFloat() * Mathf.PI * 2f;
        p.age = 0f; p.attracted = false;
        _pickups.Add(p);
    }

    int RollPickupType()
    {
        // 普通掉落池：不含武器碎片（武器只从 Boss 掉）
        int r = _rng.Next(100);
        if (r < 8) return PK_EXP; r -= 8;
        if (r < 9) return PK_DMG; r -= 9;
        if (r < 9) return PK_SPEED; r -= 9;
        if (r < 8) return PK_FIRE; r -= 8;
        if (r < 8) return PK_HP; r -= 8;
        if (r < 8) return PK_BULLET_SPD; r -= 8;
        if (r < 6) return PK_ATTRACT; r -= 6;
        if (r < 7) return PK_HEAL; r -= 7;
        if (r < 7) return PK_BOUNCE; r -= 7;
        if (r < 8) return PK_DR; r -= 8;
        if (r < 7) return PK_REGEN; r -= 7;
        if (r < 8) return PK_FIRE_AMMO; r -= 8;
        if (r < 8) return PK_ICE_AMMO; r -= 8;
        if (r < 5) return PK_RANGE; r -= 5;
        if (r < 5) return PK_KNOCKBACK;
        return PK_EXP;
    }

    Color PickupColor(int type)
    {
        switch (type)
        {
            case PK_EXP: return new Color(0.75f, 0.5f, 1f);
            case PK_DMG: return new Color(1f, 0.35f, 0.35f);
            case PK_SPEED: return new Color(0.35f, 1f, 0.45f);
            case PK_FIRE: return new Color(1f, 0.9f, 0.3f);
            case PK_HP: return new Color(1f, 0.5f, 0.8f);
            case PK_BULLET_SPD: return new Color(1f, 0.65f, 0.2f);
            case PK_ATTRACT: return new Color(0.4f, 0.7f, 1f);
            case PK_HEAL: return new Color(0.5f, 1f, 0.8f);
            case PK_BOUNCE: return new Color(0.2f, 1f, 1f);
            case PK_DR: return new Color(0.6f, 0.6f, 0.9f);
            case PK_REGEN: return new Color(0.2f, 1f, 0.5f);
            case PK_FIRE_AMMO: return new Color(1f, 0.35f, 0.1f);
            case PK_WEAPON: return new Color(1f, 1f, 1f);
            case PK_RANGE: return new Color(0.9f, 0.9f, 0.5f);
            case PK_KNOCKBACK: return new Color(1f, 0.6f, 0.9f);
            default: return new Color(0.35f, 0.75f, 1f);
        }
    }

    int NextLockedWeapon()
    {
        for (int i = 1; i < WEAPON_COUNT; i++)
            if (!_unlockedWeapons[i]) return i;
        return -1;
    }

    string PickupLabel(int type)
    {
        switch (type)
        {
            case PK_EXP: return $"+{Mathf.RoundToInt(ExpValueForLevel())} 经验";
            case PK_DMG: return $"+{PK_DMG_ADD:F1} 攻击";
            case PK_SPEED: return $"+{PK_SPEED_ADD:F1} 移速";
            case PK_FIRE:
                if (_playerFireRate >= PLAYER_MAX_FIRE_RATE - 0.01f) return "攻速 已满";
                return $"+{PK_FIRE_RATE_ADD:F2} 攻速";
            case PK_HP: return $"+{Mathf.RoundToInt(PK_HP_ADD)} 最大HP";
            case PK_BULLET_SPD:
                if (_bulletSpeed >= BULLET_SPEED_MAX - 0.01f) return "子弹速度 已满";
                return $"+{PK_BULLET_SPD_ADD:F0} 弹速";
            case PK_ATTRACT: return $"+{PK_ATTRACT_ADD:F1} 拾取范围";
            case PK_HEAL: return $"+{Mathf.RoundToInt(PK_HEAL_AMOUNT)} 回复HP";
            case PK_BOUNCE:
                if (_bounceCount >= MAX_BOUNCE) return "反弹次数 已满";
                return "+1 反弹次数";
            case PK_DR: return $"+{Mathf.RoundToInt(PK_DR_ADD * 100f)}% 减伤";
            case PK_REGEN: return $"+{PK_REGEN_ADD:F1} 每秒回血";
            case PK_FIRE_AMMO: return _fireLevel == 0 ? "获得 火焰弹" : $"火焰弹 Lv{_fireLevel + 1}";
            case PK_WEAPON:
                {
                    int w = NextLockedWeapon();
                    if (w < 0) return "武器已全解锁";
                    return $"解锁 武器：{WeaponName(w)}";
                }
            case PK_RANGE:
                if (_playerAttackRange >= PLAYER_MAX_RANGE - 0.01f) return "攻击距离 已满";
                return $"+{PK_RANGE_ADD:F1} 攻击距离";
            case PK_KNOCKBACK: return $"+{PK_KNOCKBACK_ADD:F1} 击退";
            default: return _iceLevel == 0 ? "获得 冰冻弹" : $"冰冻弹 Lv{_iceLevel + 1}";
        }
    }

    float ExpValueForLevel() { return 6f + _level * 2f; }

    void UpdatePickups(float dt)
    {
        if (_player == null || !_player.alive) return;
        for (int i = _pickups.Count - 1; i >= 0; i--)
        {
            var p = _pickups[i];
            if (p == null || p.go == null) { _pickups.RemoveAt(i); continue; }
            p.age += dt;
            Vector2 toPlayer = _player.pos - p.pos;
            float dist = toPlayer.magnitude;
            if (dist < _attractRadius) p.attracted = true;
            if (p.attracted)
            {
                float speed = Mathf.Lerp(14f, 30f, 1f - Mathf.Clamp01(dist / _attractRadius));
                if (dist > 0.0001f) p.pos += (toPlayer / dist) * speed * dt;
                if (dist < PICKUP_RADIUS)
                {
                    ApplyPickup(p.type);
                    UnityEngine.Object.Destroy(p.go);
                    _pickups.RemoveAt(i);
                    continue;
                }
            }
            float bob = Mathf.Sin(p.age * 5f + p.bobPhase) * 0.15f;
            p.tr.localPosition = new Vector3(p.pos.x, p.pos.y + bob, -0.05f);
            p.tr.localRotation = Quaternion.Euler(0f, 0f, p.age * 90f);
        }
    }

    void ApplyPickup(int type)
    {
        _pickupCount++;
        switch (type)
        {
            case PK_EXP: GainExp(ExpValueForLevel()); break;
            case PK_DMG: _playerDamage += PK_DMG_ADD; break;
            case PK_SPEED: _playerSpeed += PK_SPEED_ADD; break;
            case PK_FIRE:
                _playerFireRate = Mathf.Min(PLAYER_MAX_FIRE_RATE, _playerFireRate + PK_FIRE_RATE_ADD);
                break;
            case PK_HP:
                _playerMaxHp += PK_HP_ADD;
                _playerHp = Mathf.Min(_playerMaxHp, _playerHp + PK_HP_ADD);
                break;
            case PK_BULLET_SPD:
                _bulletSpeed = Mathf.Min(BULLET_SPEED_MAX, _bulletSpeed + PK_BULLET_SPD_ADD);
                break;
            case PK_ATTRACT: _attractRadius += PK_ATTRACT_ADD; break;
            case PK_HEAL: _playerHp = Mathf.Min(_playerMaxHp, _playerHp + PK_HEAL_AMOUNT); break;
            case PK_BOUNCE: _bounceCount = Mathf.Min(MAX_BOUNCE, _bounceCount + 1); break;
            case PK_DR: _damageReduce = Mathf.Min(MAX_DAMAGE_REDUCE, _damageReduce + PK_DR_ADD); break;
            case PK_REGEN: _hpRegen += PK_REGEN_ADD; break;
            case PK_FIRE_AMMO: _fireLevel++; break;
            case PK_ICE_AMMO: _iceLevel++; break;
            case PK_WEAPON:
                {
                    int w = NextLockedWeapon();
                    if (w >= 0) { _unlockedWeapons[w] = true; SwitchWeapon(w); }
                }
                break;
            case PK_RANGE:
                _playerAttackRange = Mathf.Min(PLAYER_MAX_RANGE, _playerAttackRange + PK_RANGE_ADD);
                break;
            case PK_KNOCKBACK:
                _playerKnockback += PK_KNOCKBACK_ADD;
                break;
        }
        _player.maxHp = _playerMaxHp;
        _player.hp = _playerHp;
        _camShake = Mathf.Max(_camShake, 0.08f);
        PlaySfx(SfxGen.Pickup(), 0.35f, 0.95f + NextFloat() * 0.1f);
    }

    void GainExp(float amount)
    {
        _playerExp += amount;
        while (_playerExp >= _playerExpNext)
        {
            _playerExp -= _playerExpNext;
            _playerLevel++;
            _playerExpNext = ExpNext(_playerLevel);

            if (_playerSides < MAX_SIDES)
            {
                _playerSides++;
                _playerRadius = PLAYER_BASE_RADIUS + (_playerSides - 3) * 0.05f;
                _player.sides = _playerSides;
                _player.radius = _playerRadius;
                _player.tr.GetComponent<MeshFilter>().sharedMesh = VMesh.Poly(_playerSides);
                _player.tr.localScale = Vector3.one * _playerRadius;
            }
            _playerMaxHp += EXP_UPGRADE_SIDES_HP;
            _playerHp = Mathf.Min(_playerMaxHp, _playerHp + EXP_UPGRADE_HEAL);
            _levelUpFlash = 1f;
            _camShake = Mathf.Max(_camShake, 0.2f);
            PlaySfx(SfxGen.LevelUp(), 0.5f, 1f);
        }
        _player.maxHp = _playerMaxHp;
        _player.hp = _playerHp;
    }

    // =====================================================================
    //  武器
    // =====================================================================
    void FireWithWeapon(Unit owner, bool fromPlayer)
    {
        if (!fromPlayer) { FireMultiArrow(owner, false, 1f, 1f); return; }
        switch (_currentWeapon)
        {
            case 1: FireShotgun(owner); break;
            case 2: FireBoomerang(owner); break;
            case 3: FireExplosive(owner); break;
            default: FireMultiArrow(owner, true, 1f, 1f); break;
        }
    }

    void FireMultiArrow(Unit owner, bool fromPlayer, float dmgMul, float speedMul)
    {
        int sides = Mathf.Max(3, owner.sides);
        float speed = (fromPlayer ? _bulletSpeed : ENEMY_BULLET_SPEED) * speedMul;
        float dmg = (fromPlayer ? _playerDamage * ComboMul() : owner.damage * 0.55f) * dmgMul;
        if (_currentMod == LevelMod.GlassCannon && fromPlayer) dmg *= 1.5f;
        float size = owner.radius * 1.15f;

        Color col = fromPlayer ? WeaponColor(_currentWeapon) : new Color(1f, 0.5f, 0.35f);
        if (fromPlayer)
        {
            if (_fireLevel > 0) col = new Color(1f, 0.55f, 0.15f);
            else if (_iceLevel > 0) col = new Color(0.55f, 0.85f, 1f);
        }

        if (fromPlayer) PlayShootSfx();
        else PlaySfx(SfxGen.EnemyShoot(), 0.14f, 0.9f + NextFloat() * 0.2f);

        float baseAngle = owner.dir.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(owner.dir.y, owner.dir.x)
            : Mathf.PI * 0.5f;

        for (int i = 0; i < sides; i++)
        {
            float a = baseAngle + i * Mathf.PI * 2f / sides;
            Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Vector2 start = owner.pos + dir * (owner.radius + 0.1f);
            var arrow = CreateArrow(start, dir, size, dmg, speed, fromPlayer, col);
            arrow.originPos = start;
            arrow.maxRange = fromPlayer ? _playerAttackRange : ENEMY_BULLET_RANGE;
            InitArrowPlayerProps(arrow, fromPlayer);
            _arrows.Add(arrow);
        }
    }

    void FireShotgun(Unit owner)
    {
        int sides = Mathf.Max(3, owner.sides);
        float speed = _bulletSpeed * 1.05f;
        float dmg = _playerDamage * 0.55f * ComboMul();
        if (_currentMod == LevelMod.GlassCannon) dmg *= 1.5f;
        float size = owner.radius * 0.85f;
        Color col = _fireLevel > 0 ? new Color(1f, 0.55f, 0.15f) :
                    _iceLevel > 0 ? new Color(0.55f, 0.85f, 1f) :
                                    WeaponColor(_currentWeapon);
        PlayShootSfx();

        float baseAngle = owner.dir.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(owner.dir.y, owner.dir.x)
            : Mathf.PI * 0.5f;

        int bulletsPerSide = 3;
        for (int s = 0; s < sides; s++)
        {
            float sideAngle = baseAngle + s * Mathf.PI * 2f / sides;
            for (int b = 0; b < bulletsPerSide; b++)
            {
                float offset = (b - (bulletsPerSide - 1) * 0.5f) * SHOTGUN_SPREAD;
                float a = sideAngle + offset;
                Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                Vector2 start = owner.pos + dir * (owner.radius + 0.1f);
                var arrow = CreateArrow(start, dir, size, dmg, speed, true, col);
                arrow.originPos = start;
                arrow.maxRange = _playerAttackRange;
                InitArrowPlayerProps(arrow, true);
                _arrows.Add(arrow);
            }
        }
    }

    void FireBoomerang(Unit owner)
    {
        int sides = Mathf.Max(3, owner.sides);
        float speed = _bulletSpeed * 1.3f;
        float dmg = _playerDamage * 0.9f * ComboMul();
        if (_currentMod == LevelMod.GlassCannon) dmg *= 1.5f;
        float size = owner.radius * 1.3f;
        Color col = WeaponColor(_currentWeapon);
        PlayShootSfx();

        float baseAngle = owner.dir.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(owner.dir.y, owner.dir.x)
            : Mathf.PI * 0.5f;

        for (int i = 0; i < sides; i++)
        {
            float a = baseAngle + i * Mathf.PI * 2f / sides;
            Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Vector2 start = owner.pos + dir * (owner.radius + 0.1f);
            var arrow = CreateArrow(start, dir, size, dmg, speed, true, col);
            arrow.isBoomerang = true;
            arrow.boomerangPhase = 0;
            arrow.boomerangOrigin = owner.pos;
            arrow.life = 4f;
            arrow.originPos = start;
            arrow.maxRange = 999f;
            InitArrowPlayerProps(arrow, true);
            _arrows.Add(arrow);
        }
    }

    void FireExplosive(Unit owner)
    {
        int sides = Mathf.Max(3, owner.sides);
        float speed = _bulletSpeed * 0.9f;
        float dmg = _playerDamage * 0.85f * ComboMul();
        if (_currentMod == LevelMod.GlassCannon) dmg *= 1.5f;
        float size = owner.radius * 1.25f;
        Color col = WeaponColor(_currentWeapon);
        PlayShootSfx();

        float baseAngle = owner.dir.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(owner.dir.y, owner.dir.x)
            : Mathf.PI * 0.5f;

        for (int i = 0; i < sides; i++)
        {
            float a = baseAngle + i * Mathf.PI * 2f / sides;
            Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Vector2 start = owner.pos + dir * (owner.radius + 0.1f);
            var arrow = CreateArrow(start, dir, size, dmg, speed, true, col);
            arrow.isExplosive = true;
            arrow.originPos = start;
            arrow.maxRange = _playerAttackRange;
            InitArrowPlayerProps(arrow, true);
            _arrows.Add(arrow);
        }
    }

    void PlayShootSfx()
    {
        if (Time.unscaledTime - _lastShootSfxTime > 0.05f)
        {
            PlaySfx(SfxGen.Shoot(), 0.20f, 0.9f + NextFloat() * 0.2f);
            _lastShootSfxTime = Time.unscaledTime;
        }
    }

    void InitArrowPlayerProps(Arrow a, bool fromPlayer)
    {
        if (!fromPlayer) return;
        a.bounce = _bounceCount;
        a.isFire = _fireLevel > 0;
        a.isIce = _iceLevel > 0;
        a.burnDps = CurrentBurnDps();
        a.burnDuration = BURN_DURATION;
        a.slowFactor = CurrentSlowFactor();
        a.slowDuration = SLOW_DURATION;
        a.isSplitChild = false;
        a.knockback = _playerKnockback;
        if (a.isFire || a.isIce) AttachArrowParticles(a);
    }

    void AttachArrowParticles(Arrow a)
    {
        if (a == null || a.go == null) return;
        var psGO = new GameObject("Particles");
        psGO.transform.SetParent(a.go.transform, false);
        psGO.transform.localScale = Vector3.one;

        var ps = psGO.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1f; main.loop = true;
        main.startLifetime = a.isFire ? 0.45f : 0.55f;
        main.startSpeed = a.isFire ? 2.2f : 1.6f;
        main.startSize = a.isFire ? 0.42f : 0.38f;
        main.startColor = a.isFire ? new Color(1f, 0.7f, 0.3f) : new Color(0.55f, 0.85f, 1f);
        main.gravityModifier = a.isFire ? -0.4f : 0f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.maxParticles = 60;
        main.playOnAwake = false;

        var emission = ps.emission;
        emission.rateOverTime = a.isFire ? 45f : 35f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.08f; shape.radiusThickness = 1f;
        shape.rotation = new Vector3(a.isFire ? 0f : -90f, 0f, 0f);

        var colLife = ps.colorOverLifetime;
        colLife.enabled = true;
        Gradient grad = new Gradient();
        if (a.isFire)
        {
            grad.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(1f, 0.98f, 0.8f), 0f),
                    new GradientColorKey(new Color(1f, 0.6f, 0.15f), 0.4f),
                    new GradientColorKey(new Color(0.5f, 0.12f, 0.03f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.75f, 0.4f),
                    new GradientAlphaKey(0f, 1f)
                });
        }
        else
        {
            grad.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(0.95f, 1f, 1f), 0f),
                    new GradientColorKey(new Color(0.55f, 0.85f, 1f), 0.4f),
                    new GradientColorKey(new Color(0.12f, 0.3f, 0.7f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.75f, 0.4f),
                    new GradientAlphaKey(0f, 1f)
                });
        }
        colLife.color = new ParticleSystem.MinMaxGradient(grad);

        var sizeLife = ps.sizeOverLifetime;
        sizeLife.enabled = true;
        sizeLife.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0f, 1f, 1f, 0f));

        var velLife = ps.velocityOverLifetime;
        velLife.enabled = true;
        velLife.space = ParticleSystemSimulationSpace.Local;
        float vy = a.isFire ? 1.2f : 0.6f;
        velLife.x = new ParticleSystem.MinMaxCurve(0f, 0f);
        velLife.y = new ParticleSystem.MinMaxCurve(vy, vy);
        velLife.z = new ParticleSystem.MinMaxCurve(0f, 0f);

        var rotLife = ps.rotationOverLifetime;
        rotLife.enabled = true;
        float rotSpeed = a.isFire ? 90f : -180f;
        rotLife.z = new ParticleSystem.MinMaxCurve(-rotSpeed * Mathf.Deg2Rad, rotSpeed * Mathf.Deg2Rad);

        var renderer = psGO.GetComponent<ParticleSystemRenderer>();
        renderer.material = VMesh.ParticleMat();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.alignment = ParticleSystemRenderSpace.View;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.sortingOrder = 50;

        ps.Play();
        a.particlesGO = psGO;
    }

    // =====================================================================
    //  子弹更新
    // =====================================================================
    void UpdateArrows(float dt)
    {
        float halfW = _worldW * 0.5f;
        float halfH = WORLD_H * 0.5f;
        float bulletR = 0.05f;

        for (int i = _arrows.Count - 1; i >= 0; i--)
        {
            var a = _arrows[i];
            if (a == null || a.go == null) { _arrows.RemoveAt(i); continue; }

            a.life -= dt;
            if (a.life <= 0f) { UnityEngine.Object.Destroy(a.go); _arrows.RemoveAt(i); continue; }
            if (a.hitCooldown > 0f) a.hitCooldown -= dt;

            Vector2 oldPos = a.pos;

            if (a.isBoomerang && a.fromPlayer)
            {
                if (a.boomerangPhase == 0)
                {
                    float traveled = (a.pos - a.boomerangOrigin).magnitude;
                    if (traveled >= BOOMERANG_MAX_DIST) a.boomerangPhase = 1;
                }
                else
                {
                    if (_player != null && _player.alive)
                    {
                        Vector2 toP = _player.pos - a.pos;
                        float d = toP.magnitude;
                        if (d < 0.8f)
                        {
                            UnityEngine.Object.Destroy(a.go);
                            _arrows.RemoveAt(i);
                            continue;
                        }
                        Vector2 dir = toP / Mathf.Max(0.0001f, d);
                        a.vel = dir * a.vel.magnitude;
                        a.tr.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(a.vel.y, a.vel.x) * Mathf.Rad2Deg - 90f);
                    }
                }
                a.hitCooldown = 0f;
            }

            Vector2 newPos = a.pos + a.vel * dt;

            // 攻击距离限制
            if (!a.isBoomerang && a.maxRange > 0f)
            {
                if ((newPos - a.originPos).sqrMagnitude > a.maxRange * a.maxRange)
                {
                    UnityEngine.Object.Destroy(a.go);
                    _arrows.RemoveAt(i);
                    continue;
                }
            }

            bool outX = newPos.x - bulletR < -halfW || newPos.x + bulletR > halfW;
            bool outY = newPos.y - bulletR < -halfH || newPos.y + bulletR > halfH;

            if (outX || outY)
            {
                if (a.isBoomerang && a.fromPlayer && a.boomerangPhase == 0)
                {
                    a.boomerangPhase = 1;
                }
                else if (a.bounce > 0)
                {
                    if (outX)
                    {
                        if (newPos.x - bulletR < -halfW) newPos.x = -halfW + bulletR;
                        else newPos.x = halfW - bulletR;
                        a.vel.x = -a.vel.x;
                    }
                    if (outY)
                    {
                        if (newPos.y - bulletR < -halfH) newPos.y = -halfH + bulletR;
                        else newPos.y = halfH - bulletR;
                        a.vel.y = -a.vel.y;
                    }
                    a.bounce--;
                    PlaySfx(SfxGen.Bounce(), 0.28f, 0.95f + NextFloat() * 0.1f);
                }
                else { UnityEngine.Object.Destroy(a.go); _arrows.RemoveAt(i); continue; }
            }

            MapObject hitObs;
            if (LineHitsObstacle(oldPos, newPos, out hitObs))
            {
                if (a.bounce > 0)
                {
                    Vector2 n = newPos - hitObs.pos;
                    if (n.sqrMagnitude < 0.0001f) n = -a.vel.normalized;
                    n = n.normalized;
                    a.vel = Vector2.Reflect(a.vel, n);
                    newPos = hitObs.pos + n * (hitObs.radius + a.size * 0.35f + 0.05f);
                    a.bounce--;
                    a.hitCooldown = 0.06f;
                    PlaySfx(SfxGen.Bounce(), 0.28f, 0.95f + NextFloat() * 0.1f);
                }
                else
                {
                    UnityEngine.Object.Destroy(a.go);
                    _arrows.RemoveAt(i);
                    continue;
                }
            }

            bool dead = false;
            if (a.hitCooldown <= 0f)
            {
                if (a.fromPlayer)
                {
                    for (int j = _enemies.Count - 1; j >= 0; j--)
                    {
                        var e = _enemies[j];
                        if (e == null || !e.alive) continue;
                        float rr = e.radius + a.size * 0.35f;
                        float d = PointToSegmentDist(e.pos, oldPos, newPos);
                        if (d < rr)
                        {
                            e.hp -= a.damage;
                            e.hitFlash = 0.1f;

                            // 击退
                            if (a.knockback > 0f && !e.isBoss)
                            {
                                Vector2 kbDir = a.vel.normalized;
                                e.pos += kbDir * a.knockback;
                                float hw = _worldW * 0.5f - e.radius;
                                float hh = WORLD_H * 0.5f - e.radius;
                                e.pos.x = Mathf.Clamp(e.pos.x, -hw, hw);
                                e.pos.y = Mathf.Clamp(e.pos.y, -hh, hh);
                            }

                            PlaySfx(SfxGen.Hit(), 0.25f, 0.95f + NextFloat() * 0.1f);
                            if (a.isFire) { e.burnTime = Mathf.Max(e.burnTime, a.burnDuration); e.burnDps = a.burnDps; }
                            if (a.isIce) { e.slowTime = Mathf.Max(e.slowTime, a.slowDuration); e.slowFactor = a.slowFactor; }

                            if (a.isExplosive)
                                ExplodeAt(newPos, EXPLOSIVE_RADIUS, a.damage * EXPLOSIVE_DMG_MUL);

                            if (e.hp <= 0f) KillEnemy(e);

                            if (_synSplit && !a.isSplitChild)
                            {
                                Vector2 n = newPos - e.pos;
                                if (n.sqrMagnitude < 0.0001f) n = -a.vel.normalized;
                                n = n.normalized;
                                Vector2 childDir = new Vector2(-n.y, n.x);
                                Vector2 childStart = e.pos + n * (e.radius + a.size * 0.35f + 0.1f);
                                Color childCol = a.isFire ? new Color(1f, 0.55f, 0.15f) :
                                                 a.isIce ? new Color(0.55f, 0.85f, 1f) :
                                                           new Color(0.45f, 1f, 0.65f);
                                var child = CreateArrow(childStart, childDir, a.size * 0.7f, a.damage * 0.5f, a.vel.magnitude * 0.9f, true, childCol);
                                child.bounce = Mathf.Max(0, a.bounce);
                                child.isFire = a.isFire; child.isIce = a.isIce;
                                child.burnDps = a.burnDps; child.burnDuration = a.burnDuration;
                                child.slowFactor = a.slowFactor; child.slowDuration = a.slowDuration;
                                child.knockback = a.knockback;
                                child.isSplitChild = true;
                                child.originPos = childStart;
                                child.maxRange = a.maxRange;
                                if (child.isFire || child.isIce) AttachArrowParticles(child);
                                _arrows.Add(child);
                            }

                            if (a.isBoomerang) a.hitCooldown = 0.1f;
                            else if (a.bounce > 0)
                            {
                                Vector2 normal = newPos - e.pos;
                                if (normal.sqrMagnitude < 0.0001f) normal = -a.vel.normalized;
                                normal = normal.normalized;
                                a.vel = Vector2.Reflect(a.vel, normal);
                                newPos = e.pos + normal * (e.radius + a.size * 0.35f + 0.05f);
                                a.bounce--;
                                a.hitCooldown = 0.06f;
                                PlaySfx(SfxGen.Bounce(), 0.28f, 0.95f + NextFloat() * 0.1f);
                            }
                            else dead = true;
                            break;
                        }
                    }
                }
                else if (_player != null && _player.alive)
                {
                    float rr = _player.radius + a.size * 0.35f;
                    float d = PointToSegmentDist(_player.pos, oldPos, newPos);
                    if (d < rr) { DamagePlayer(a.damage); dead = true; }
                }
            }

            if (!dead)
            {
                float boundX = halfW + 8f;
                float boundY = halfH + 8f;
                if (Mathf.Abs(newPos.x) > boundX || Mathf.Abs(newPos.y) > boundY) dead = true;
            }

            if (dead) { UnityEngine.Object.Destroy(a.go); _arrows.RemoveAt(i); continue; }

            a.pos = newPos;
            a.tr.localPosition = new Vector3(a.pos.x, a.pos.y, 0f);
            float ang = Mathf.Atan2(a.vel.y, a.vel.x) * Mathf.Rad2Deg - 90f;
            a.tr.localRotation = Quaternion.Euler(0f, 0f, ang);
        }

        ResolveBulletCollisions();
    }

    // ★ 子弹抵消：安全版本，先收集再删除
    void ResolveBulletCollisions()
    {
        if (_arrows.Count < 2) return;

        _bulletCollisionBuffer.Clear();

        for (int i = 0; i < _arrows.Count; i++)
        {
            var pa = _arrows[i];
            if (pa == null || pa.go == null || !pa.fromPlayer) continue;
            if (_bulletCollisionBuffer.Contains(pa)) continue;

            float pr = pa.size * 0.4f;
            for (int j = 0; j < _arrows.Count; j++)
            {
                if (i == j) continue;
                var ea = _arrows[j];
                if (ea == null || ea.go == null || ea.fromPlayer) continue;
                if (_bulletCollisionBuffer.Contains(ea)) continue;

                float er = ea.size * 0.4f;
                float rr = pr + er;
                if ((pa.pos - ea.pos).sqrMagnitude < rr * rr)
                {
                    _bulletCollisionBuffer.Add(pa);
                    _bulletCollisionBuffer.Add(ea);
                    break;
                }
            }
        }

        if (_bulletCollisionBuffer.Count == 0) return;

        PlaySfx(SfxGen.Bounce(), 0.22f, 1.4f);

        for (int k = 0; k < _bulletCollisionBuffer.Count; k++)
        {
            var a = _bulletCollisionBuffer[k];
            if (a == null) continue;
            if (a.go != null) UnityEngine.Object.Destroy(a.go);
            _arrows.Remove(a);
        }
        _bulletCollisionBuffer.Clear();
    }

    void ExplodeAt(Vector2 pos, float radius, float dmg)
    {
        float r2 = radius * radius;
        _killBuffer.Clear();
        for (int i = 0; i < _enemies.Count; i++)
        {
            var e = _enemies[i];
            if (e == null || !e.alive) continue;
            if ((e.pos - pos).sqrMagnitude < r2)
            {
                e.hp -= dmg;
                if (e.hp <= 0f) _killBuffer.Add(e);
            }
        }
        for (int k = 0; k < _killBuffer.Count; k++)
        {
            var e = _killBuffer[k];
            if (e != null && e.alive) KillEnemy(e);
        }
        _killBuffer.Clear();
        _camShake = Mathf.Max(_camShake, 0.25f);
        PlaySfx(SfxGen.Kill(), 0.35f, 1.2f);
    }

    void DamagePlayer(float dmg)
    {
        if (_gameOver || _player == null || !_player.alive) return;
        if (_invulnTimer > 0f) return;

        float finalDmg = dmg * (1f - _damageReduce);
        if (_currentMod == LevelMod.GlassCannon) finalDmg *= 1.5f;

        _playerHp -= finalDmg;
        if (_playerHp < 0f) _playerHp = 0f;
        _player.hp = _playerHp;
        _hurtFlash = 1f;
        _camShake = Mathf.Max(_camShake, 0.28f);

        _combo = 0;
        _comboTimer = 0f;
        PlaySfx(SfxGen.Hurt(), 0.4f, 1f);

        if (_playerHp <= 0f)
        {
            _player.alive = false;
            if (_player.go != null) UnityEngine.Object.Destroy(_player.go);
            _gameOver = true;
            _gameOverTimer = 3f;
            _camShake = 0.8f;
        }
    }

    void UpdateCamera(float dt)
    {
        _camShake = Mathf.Max(0f, _camShake - dt * 2.2f);
        if (_camTarget == null) return;
        Vector3 shakeOffset = Vector3.zero;
        if (_camShake > 0f)
        {
            float s = _camShake * 0.45f;
            shakeOffset.x = (NextFloat() * 2f - 1f) * s;
            shakeOffset.y = (NextFloat() * 2f - 1f) * s;
        }
        _camTarget.localPosition = shakeOffset;
    }

    Unit CreateUnit(int sides, float radius, Color color, string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(_root, false);
        go.transform.localScale = Vector3.one * radius;
        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = VMesh.Poly(sides);
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = VMesh.Mat(color);
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.lightProbeUsage = LightProbeUsage.Off;
        mr.reflectionProbeUsage = ReflectionProbeUsage.Off;

        var u = new Unit();
        u.go = go; u.tr = go.transform; u.mr = mr;
        u.sides = sides; u.radius = radius; u.color = color;
        u.alive = true; u.hp = 1f; u.maxHp = 1f;
        u.kind = EnemyKind.Normal;
        return u;
    }

    Arrow CreateArrow(Vector2 pos, Vector2 dir, float size, float damage, float speed, bool fromPlayer, Color color)
    {
        var go = new GameObject(fromPlayer ? "PArrow" : "EArrow");
        go.transform.SetParent(_root, false);
        go.transform.localScale = Vector3.one * size;
        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = VMesh.Arrow();
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = VMesh.Mat(color);
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.lightProbeUsage = LightProbeUsage.Off;
        mr.reflectionProbeUsage = ReflectionProbeUsage.Off;

        var a = new Arrow();
        a.go = go; a.tr = go.transform;
        a.pos = pos; a.vel = dir.normalized * speed;
        a.damage = damage; a.life = 2.6f;
        a.size = size; a.fromPlayer = fromPlayer;
        a.originPos = pos;
        a.maxRange = fromPlayer ? _playerAttackRange : ENEMY_BULLET_RANGE;
        a.tr.localPosition = new Vector3(pos.x, pos.y, 0f);
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        a.tr.localRotation = Quaternion.Euler(0f, 0f, ang);
        return a;
    }

    float NextFloat() { return (float)_rng.NextDouble(); }

    // =====================================================================
    //  HUD
    // =====================================================================
    public void DrawHUD()
    {
        int fs = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.026f), 12, 28);
        int small = Mathf.Max(11, fs - 4);
        float lineH = Mathf.RoundToInt(small * 1.6f) + 10f;

        if (_hurtFlash > 0f)
        {
            var prev = GUI.color;
            GUI.color = new Color(1f, 0.1f, 0.1f, _hurtFlash * 0.22f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }
        if (_levelUpFlash > 0f)
        {
            var prev = GUI.color;
            GUI.color = new Color(1f, 0.95f, 0.4f, _levelUpFlash * 0.18f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }
        if (_burstFlash > 0f)
        {
            var prev = GUI.color;
            GUI.color = new Color(1f, 1f, 0.5f, _burstFlash * 0.3f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }
        if (_timeStopTimer > 0f)
        {
            var prev = GUI.color;
            GUI.color = new Color(0.3f, 0.6f, 1f, 0.12f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }

        DrawBossBar();
        DrawWorldBarsAndPickups();
        DrawSkillIndicators();

        float x = 20f;
        float barX = 95f;
        float barW = 220f;
        float barH = 12f;
        float y = 14f;
        if (_boss != null && _boss.alive) y += 60f;

        Label($"关卡 {_level}" + (_inLevelBreak ? "  ·  准备中..." : (_isBossLevel ? "  ·  BOSS" : "")),
              x, y, fs, _isBossLevel && !_inLevelBreak ? new Color(1f, 0.4f, 0.5f) : Color.white);
        y += Mathf.RoundToInt(fs * 1.6f) + 14f;

        if (_currentMod != LevelMod.None && !_inLevelBreak)
        {
            Color modCol = new Color(1f, 0.85f, 0.3f);
            switch (_currentMod)
            {
                case LevelMod.Hardened: modCol = new Color(0.6f, 0.9f, 1f); break;
                case LevelMod.Swift: modCol = new Color(0.8f, 0.9f, 1f); break;
                case LevelMod.GlassCannon: modCol = new Color(1f, 0.55f, 0.55f); break;
                case LevelMod.BloodMoon: modCol = new Color(1f, 0.3f, 0.5f); break;
            }
            Label($"【{LevelModName(_currentMod)}】", x, y, small, modCol);
            y += lineH;
        }

        string weaponLine = "武器：";
        for (int i = 0; i < WEAPON_COUNT; i++)
        {
            if (!_unlockedWeapons[i]) continue;
            if (i == _currentWeapon) weaponLine += $"[{i + 1}]{WeaponName(i)}  ";
            else weaponLine += $"{i + 1}.{WeaponName(i)}  ";
        }
        Label(weaponLine, x, y, small, new Color(1f, 1f, 1f, 0.85f));
        y += lineH;

        Label($"等级 {_playerLevel}   击杀 {_killCount}   拾取 {_pickupCount}", x, y, small, new Color(0.7f, 0.9f, 1f));
        y += lineH;

        Label("经验", x, y, small, new Color(0.85f, 0.75f, 1f));
        DrawBar(barX, y + 4f, barW, barH, _playerExp / Mathf.Max(1f, _playerExpNext),
                new Color(0.75f, 0.5f, 1f), new Color(1f, 1f, 1f, 0.12f));
        y += lineH;

        Label("生命", x, y, small, new Color(0.9f, 0.9f, 0.9f));
        DrawBar(barX, y + 4f, barW, barH, _playerHp / Mathf.Max(1f, _playerMaxHp),
                new Color(0.95f, 0.3f, 0.35f), new Color(1f, 1f, 1f, 0.12f));
        y += lineH;

        float dashReady = 1f - Mathf.Clamp01(_dashCd / DASH_CD);
        Color dashCol = _dashCd <= 0f ? new Color(0.4f, 1f, 1f) : new Color(0.7f, 0.7f, 0.7f);
        Label("冲刺 [Shift]", x, y, small, dashCol);
        DrawBar(barX, y + 4f, barW, barH, dashReady, new Color(0.4f, 1f, 1f), new Color(1f, 1f, 1f, 0.12f));
        y += lineH;

        float burstReady = 1f - Mathf.Clamp01(_burstCd / BURST_CD);
        Color burstCol = _burstCd <= 0f ? new Color(1f, 0.7f, 0.3f) : new Color(0.7f, 0.7f, 0.7f);
        Label("爆裂 [E]", x, y, small, burstCol);
        DrawBar(barX, y + 4f, barW, barH, burstReady, new Color(1f, 0.7f, 0.3f), new Color(1f, 1f, 1f, 0.12f));
        y += lineH;

        float tsReady = 1f - Mathf.Clamp01(_timeStopCd / TIMESTOP_CD);
        Color tsCol = _timeStopCd <= 0f ? new Color(0.4f, 0.7f, 1f) : new Color(0.7f, 0.7f, 0.7f);
        Label("时停 [Q]", x, y, small, tsCol);
        DrawBar(barX, y + 4f, barW, barH, tsReady, new Color(0.4f, 0.7f, 1f), new Color(1f, 1f, 1f, 0.12f));
        y += lineH;

        Label($"HP {Mathf.CeilToInt(_playerHp)}/{Mathf.CeilToInt(_playerMaxHp)}   边数 {_playerSides}   攻击 {_playerDamage:F1}   移速 {_playerSpeed:F2}",
              x, y, small, new Color(0.8f, 0.85f, 0.95f));
        y += lineH;

        Label($"攻速 {_playerFireRate:F2}/s   弹速 {_bulletSpeed:F1}   射程 {_playerAttackRange:F1}   击退 {_playerKnockback:F1}",
              x, y, small, new Color(0.8f, 0.85f, 0.95f));
        y += lineH;

        Label($"拾取 {_attractRadius:F1}   反弹 {_bounceCount}   减伤 {Mathf.RoundToInt(_damageReduce * 100f)}%   回血 {_hpRegen:F1}",
              x, y, small, new Color(0.8f, 0.85f, 0.95f));
        y += lineH;

        if (_fireLevel > 0)
        {
            float dps = CurrentBurnDps();
            Label($"🔥 火焰弹 Lv{_fireLevel}   灼烧 {dps:F0}/s", x, y, small, new Color(1f, 0.45f, 0.15f));
            y += lineH;
        }
        if (_iceLevel > 0)
        {
            float sf = CurrentSlowFactor();
            int slowPct = Mathf.RoundToInt((1f - sf) * 100f);
            Label($"❄ 冰冻弹 Lv{_iceLevel}   减速 {slowPct}%", x, y, small, new Color(0.45f, 0.8f, 1f));
            y += lineH;
        }

        if (_synEvaporate) { Label("⚡ 协同：蒸发爆炸", x, y, small, new Color(1f, 0.7f, 0.3f)); y += lineH; }
        if (_synSplit) { Label("⚡ 协同：弹幕分裂", x, y, small, new Color(1f, 0.7f, 0.3f)); y += lineH; }
        if (_synThorns) { Label("⚡ 协同：荆棘护甲", x, y, small, new Color(1f, 0.7f, 0.3f)); y += lineH; }

        if (_combo > 1)
        {
            float comboFrac = Mathf.Clamp01(_comboTimer / COMBO_TIMEOUT);
            float mul = ComboMul();
            Color cc = mul >= 2f ? new Color(1f, 0.4f, 0.4f) :
                       mul >= 1.5f ? new Color(1f, 0.7f, 0.3f) :
                                     new Color(1f, 0.95f, 0.6f);
            Label($"连击 x{_combo}   伤害 x{mul:F2}", x, y, small, cc);
            y += lineH;
            DrawBar(barX, y + 4f, barW, 6f, comboFrac, cc, new Color(1f, 1f, 1f, 0.12f));
            y += lineH;
        }

        Label("WASD 移动 · Shift 冲刺 · E 爆裂 · Q 时停 · 1~4 切换武器",
              Screen.width - 720f, Screen.height - 40f, small, new Color(1f, 1f, 1f, 0.35f));

        if (_gameOver)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.fontSize = Mathf.RoundToInt(Screen.height * 0.075f);
            s.alignment = TextAnchor.MiddleCenter;
            s.normal.textColor = new Color(1f, 0.3f, 0.3f);
            GUI.Label(new Rect(0, Screen.height * 0.35f, Screen.width, Screen.height * 0.2f), "已被感染…", s);

            var s2 = new GUIStyle(GUI.skin.label);
            s2.fontSize = Mathf.RoundToInt(Screen.height * 0.03f);
            s2.alignment = TextAnchor.MiddleCenter;
            s2.normal.textColor = new Color(1f, 1f, 1f, 0.8f);
            GUI.Label(new Rect(0, Screen.height * 0.5f, Screen.width, 60f),
                      $"即将从第 {_level} 关重开  ·  等级 {_playerLevel}  ·  击杀 {_killCount}", s2);
        }
    }

    void DrawSkillIndicators()
    {
        if (_timeStopTimer > 0f)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.fontSize = Mathf.RoundToInt(Screen.height * 0.04f);
            s.alignment = TextAnchor.MiddleCenter;
            s.fontStyle = FontStyle.Bold;
            s.normal.textColor = new Color(0.5f, 0.85f, 1f, 0.9f);
            GUI.Label(new Rect(0, Screen.height - 120f, Screen.width, 60f),
                      $"⏸ 时停中  {_timeStopTimer:F1}s", s);
        }
    }

    void DrawBossBar()
    {
        if (_boss == null || !_boss.alive) return;
        float bw = Screen.width * 0.6f;
        float bh = 22f;
        float bx = (Screen.width - bw) * 0.5f;
        float by = 24f;

        var prev = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.9f);
        GUI.DrawTexture(new Rect(bx - 3f, by - 3f, bw + 6f, bh + 6f), Texture2D.whiteTexture);

        GUI.color = new Color(0.15f, 0.05f, 0.1f, 1f);
        GUI.DrawTexture(new Rect(bx, by, bw, bh), Texture2D.whiteTexture);

        float ratio = _boss.maxHp > 0f ? Mathf.Clamp01(_boss.hp / _boss.maxHp) : 0f;
        Color barCol = _bossPhase == 3 ? new Color(1f, 0.2f, 0.3f) :
                       _bossPhase == 2 ? new Color(1f, 0.35f, 0.5f) :
                                         new Color(0.9f, 0.25f, 0.45f);
        GUI.color = barCol;
        GUI.DrawTexture(new Rect(bx, by, bw * ratio, bh), Texture2D.whiteTexture);

        GUI.color = new Color(0f, 0f, 0f, 0.7f);
        GUI.DrawTexture(new Rect(bx + bw * 0.33f, by, 2f, bh), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(bx + bw * 0.66f, by, 2f, bh), Texture2D.whiteTexture);

        var nameStyle = new GUIStyle(GUI.skin.label);
        nameStyle.fontSize = Mathf.RoundToInt(bh * 0.85f);
        nameStyle.alignment = TextAnchor.MiddleCenter;
        nameStyle.fontStyle = FontStyle.Bold;
        nameStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(bx, by - 2f, bw, bh + 4f),
                  $"BOSS   第 {_bossPhase} 阶段   {Mathf.CeilToInt(_boss.hp)}/{Mathf.CeilToInt(_boss.maxHp)}",
                  nameStyle);
        GUI.color = prev;
    }

    void DrawWorldBarsAndPickups()
    {
        var cam = Camera.main;
        if (cam == null) return;
        float pxPerUnit = Screen.height / Mathf.Max(0.0001f, cam.orthographicSize * 2f);
        float barHeightPx = Mathf.Max(3f, BAR_WORLD_HEIGHT * pxPerUnit);

        if (_player != null && _player.alive) DrawBarFor(cam, _player, pxPerUnit, barHeightPx);

        for (int i = 0; i < _enemies.Count; i++)
        {
            var e = _enemies[i];
            if (e == null || !e.alive || e.isBoss) continue;
            DrawBarFor(cam, e, pxPerUnit, barHeightPx);
        }
        DrawMapLabels(cam);
        DrawPickupLabels(cam);
    }

    void DrawBarFor(Camera cam, Unit u, float pxPerUnit, float barHeightPx)
    {
        Vector3 world = new Vector3(u.pos.x, u.pos.y + u.radius + 0.35f, 0f);
        Vector3 sp = cam.WorldToScreenPoint(world);
        if (sp.z < 0f) return;

        float guiX = sp.x;
        float guiY = Screen.height - sp.y;
        float barWidthPx = u.radius * BAR_WORLD_WIDTH_FACTOR * pxPerUnit;
        float x = guiX - barWidthPx * 0.5f;
        float y = guiY - barHeightPx * 0.5f;
        var prev = GUI.color;

        GUI.color = new Color(0f, 0f, 0f, 0.85f);
        GUI.DrawTexture(new Rect(x, y, barWidthPx, barHeightPx), Texture2D.whiteTexture);

        float ratio = u.maxHp > 0f ? Mathf.Clamp01(u.hp / u.maxHp) : 0f;
        Color barCol = new Color(0.9f, 0.2f, 0.2f);
        if (!u.isPlayer)
        {
            switch (u.kind)
            {
                case EnemyKind.Charger: barCol = new Color(1f, 0.4f, 0.2f); break;
                case EnemyKind.Shooter: barCol = new Color(0.3f, 0.75f, 1f); break;
                case EnemyKind.Tank: barCol = new Color(0.8f, 0.5f, 1f); break;
            }
        }
        GUI.color = barCol;
        GUI.DrawTexture(new Rect(x, y, barWidthPx * ratio, barHeightPx), Texture2D.whiteTexture);

        if (u.burnTime > 0f || u.slowTime > 0f)
        {
            float statusH = Mathf.Max(2f, barHeightPx * 0.35f);
            float sy = y + barHeightPx + 1f;
            if (u.burnTime > 0f)
            {
                GUI.color = new Color(1f, 0.45f, 0.1f, 0.9f);
                GUI.DrawTexture(new Rect(x, sy, barWidthPx, statusH), Texture2D.whiteTexture);
                sy += statusH + 1f;
            }
            if (u.slowTime > 0f)
            {
                GUI.color = new Color(0.4f, 0.8f, 1f, 0.9f);
                GUI.DrawTexture(new Rect(x, sy, barWidthPx, statusH), Texture2D.whiteTexture);
            }
        }
        GUI.color = prev;
    }

    // ★ 地图元素标签
    void DrawMapLabels(Camera cam)
    {
        if (_mapObjects.Count == 0) return;
        int fontSize = Mathf.Max(12, Mathf.RoundToInt(Screen.height * 0.02f));
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.richText = false;

        for (int i = 0; i < _mapObjects.Count; i++)
        {
            var m = _mapObjects[i];
            if (m == null || !m.isAlive || m.go == null) continue;

            string text;
            Color col;
            switch (m.kind)
            {
                case MapKind.Portal: text = "传送门"; col = new Color(0.8f, 0.6f, 1f); break;
                case MapKind.EnergyPillar: text = "回血阵"; col = new Color(0.5f, 1f, 0.6f); break;
                case MapKind.Mine: text = "地雷"; col = new Color(1f, 0.6f, 0.3f); break;
                default: continue;
            }

            Vector3 world = new Vector3(m.pos.x, m.pos.y + m.radius + 0.5f, 0f);
            Vector3 sp = cam.WorldToScreenPoint(world);
            if (sp.z < 0f) continue;

            float guiX = sp.x;
            float guiY = Screen.height - sp.y;

            var shadow = new GUIStyle(style);
            shadow.normal.textColor = new Color(0f, 0f, 0f, 0.9f);
            GUI.Label(new Rect(guiX - 100f + 1.5f, guiY + 1.5f, 200f, fontSize * 1.8f + 6f), text, shadow);

            style.normal.textColor = col;
            GUI.Label(new Rect(guiX - 100f, guiY, 200f, fontSize * 1.8f + 6f), text, style);
        }
    }

    void DrawPickupLabels(Camera cam)
    {
        if (_pickups.Count == 0) return;
        int fontSize = Mathf.Max(12, Mathf.RoundToInt(Screen.height * 0.021f));
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        style.alignment = TextAnchor.MiddleCenter;
        style.richText = false;
        style.fontStyle = FontStyle.Bold;

        for (int i = 0; i < _pickups.Count; i++)
        {
            var p = _pickups[i];
            if (p == null || p.go == null) continue;
            Vector3 world = new Vector3(p.pos.x, p.pos.y + 0.8f, 0f);
            Vector3 sp = cam.WorldToScreenPoint(world);
            if (sp.z < 0f) continue;

            float guiX = sp.x;
            float guiY = Screen.height - sp.y;
            string text = PickupLabel(p.type);
            Color col = PickupColor(p.type);

            var shadow = new GUIStyle(style);
            shadow.normal.textColor = new Color(0f, 0f, 0f, 0.95f);
            GUI.Label(new Rect(guiX - 150f + 1.5f, guiY + 1.5f, 300f, fontSize * 1.8f + 6f), text, shadow);

            style.normal.textColor = col;
            GUI.Label(new Rect(guiX - 150f, guiY, 300f, fontSize * 1.8f + 6f), text, style);
        }
    }

    void Label(string text, float x, float y, int size, Color c)
    {
        var s = new GUIStyle(GUI.skin.label);
        s.fontSize = size;
        s.normal.textColor = c;
        s.richText = false;
        s.wordWrap = false;
        GUI.Label(new Rect(x, y, Screen.width - x - 10f, size * 1.8f + 8f), text, s);
    }

    void DrawBar(float x, float y, float w, float h, float fill, Color fg, Color bg)
    {
        var prev = GUI.color;
        GUI.color = bg;
        GUI.DrawTexture(new Rect(x, y, w, h), Texture2D.whiteTexture);
        GUI.color = fg;
        GUI.DrawTexture(new Rect(x, y, w * Mathf.Clamp01(fill), h), Texture2D.whiteTexture);
        GUI.color = prev;
    }
}

// =====================================================================
//  数据类
// =====================================================================
class Unit
{
    public GameObject go;
    public Transform tr;
    public MeshRenderer mr;
    public Vector2 pos;
    public Vector2 dir = Vector2.up;
    public int sides;
    public float radius;
    public float hp, maxHp;
    public float speed;
    public float damage;
    public float fireCd;
    public float shootCd;
    public float shootInterval;
    public bool canShoot;
    public float hitFlash;
    public bool isPlayer;
    public bool alive = true;
    public Color color;
    public float burnTime;
    public float burnDps;
    public float slowTime;
    public float slowFactor;
    public ParticleSystem fireFx;
    public ParticleSystem iceFx;
    public EnemyKind kind;
    public float aiTimer;
    public bool chargeReady;
    public Vector2 chargeDir;
    public bool isSplit;
    public bool isBoss;
}

class Arrow
{
    public GameObject go;
    public Transform tr;
    public Vector2 pos;
    public Vector2 vel;
    public float damage;
    public float life;
    public float size;
    public bool fromPlayer;
    public int bounce;
    public float hitCooldown;
    public bool isFire;
    public bool isIce;
    public float burnDps;
    public float burnDuration;
    public float slowFactor;
    public float slowDuration;
    public bool isSplitChild;
    public GameObject particlesGO;
    public bool isBoomerang;
    public int boomerangPhase;
    public Vector2 boomerangOrigin;
    public bool isExplosive;
    public Vector2 originPos;
    public float maxRange;
    public float knockback;
}

class Pickup
{
    public GameObject go;
    public Transform tr;
    public Vector2 pos;
    public int type;
    public float bobPhase;
    public float age;
    public bool attracted;
}

class MapObject
{
    public GameObject go;
    public Transform tr;
    public Vector2 pos;
    public MapKind kind;
    public float radius;
    public float baseScale;
    public float age;
    public bool isAlive;
    public int pairIndex = -1;
    public Color color;
}

class VirusHUD : MonoBehaviour
{
    public VirusScene scene;
    void OnGUI() { if (scene != null) scene.DrawHUD(); }
}

// =====================================================================
//  网格 & 材质工厂
// =====================================================================
static class VMesh
{
    static readonly Dictionary<int, Mesh> _polyCache = new Dictionary<int, Mesh>();
    static readonly Dictionary<int, Material> _matCache = new Dictionary<int, Material>();
    static Mesh _arrow;
    static Texture2D _circleTex;
    static Material _particleMat;

    public static Mesh Poly(int sides)
    {
        sides = Mathf.Clamp(sides, 3, 64);
        if (_polyCache.TryGetValue(sides, out var cached) && cached != null) return cached;
        var verts = new List<Vector3>();
        var tris = new List<int>();
        var uvs = new List<Vector2>();
        verts.Add(Vector3.zero);
        uvs.Add(new Vector2(0.5f, 0.5f));
        for (int i = 0; i < sides; i++)
        {
            float a = Mathf.PI * 2f * i / sides + Mathf.PI * 0.5f;
            float cx = Mathf.Cos(a);
            float sy = Mathf.Sin(a);
            verts.Add(new Vector3(cx, sy, 0f));
            uvs.Add(new Vector2(cx * 0.5f + 0.5f, sy * 0.5f + 0.5f));
        }
        for (int i = 0; i < sides; i++)
        {
            int a = i + 1;
            int b = (i + 1) % sides + 1;
            tris.Add(0); tris.Add(a); tris.Add(b);
            tris.Add(0); tris.Add(b); tris.Add(a);
        }
        var mesh = new Mesh();
        mesh.name = "VirusPoly" + sides;
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateBounds();
        _polyCache[sides] = mesh;
        return mesh;
    }

    public static Mesh Arrow()
    {
        if (_arrow != null) return _arrow;
        Vector3 tip = new Vector3(0.00f, 1.00f, 0f);
        Vector3 left = new Vector3(-0.38f, -0.28f, 0f);
        Vector3 notch = new Vector3(0.00f, -0.10f, 0f);
        Vector3 right = new Vector3(0.38f, -0.28f, 0f);
        var verts = new List<Vector3> { tip, left, notch, right };
        var tris = new List<int> { 0, 1, 2, 0, 2, 1, 0, 2, 3, 0, 3, 2 };
        var mesh = new Mesh();
        mesh.name = "VirusArrow";
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateBounds();
        _arrow = mesh;
        return _arrow;
    }

    public static Texture2D CircleTex()
    {
        if (_circleTex != null) return _circleTex;
        int size = 64;
        _circleTex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        _circleTex.name = "VirusCircleTex";
        _circleTex.wrapMode = TextureWrapMode.Clamp;
        _circleTex.filterMode = FilterMode.Bilinear;
        var pixels = new Color[size * size];
        float c = size * 0.5f;
        float r = c - 1f;
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = x - c + 0.5f;
                float dy = y - c + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy) / r;
                float a = Mathf.Clamp01(1f - d);
                a = a * a * (3f - 2f * a);
                pixels[y * size + x] = new Color(1f, 1f, 1f, a);
            }
        _circleTex.SetPixels(pixels);
        _circleTex.Apply();
        return _circleTex;
    }

    public static Material ParticleMat()
    {
        if (_particleMat != null) return _particleMat;
        Shader shader = Shader.Find("HDRP/Particles/Unlit");
        if (shader == null) shader = Shader.Find("Particles/Standard Unlit");
        if (shader == null) shader = Shader.Find("Legacy Shaders/Particles/Alpha Blended");
        if (shader == null) shader = Shader.Find("Sprites/Default");
        if (shader == null) shader = Shader.Find("Unlit/Transparent");

        _particleMat = new Material(shader);
        _particleMat.name = "VirusParticleMat";
        var tex = CircleTex();
        if (_particleMat.HasProperty("_MainTex")) _particleMat.SetTexture("_MainTex", tex);
        if (_particleMat.HasProperty("_BaseColorMap")) _particleMat.SetTexture("_BaseColorMap", tex);
        if (_particleMat.HasProperty("_UnlitColorMap")) _particleMat.SetTexture("_UnlitColorMap", tex);
        if (_particleMat.HasProperty("_BaseMap")) _particleMat.SetTexture("_BaseMap", tex);
        if (_particleMat.HasProperty("_BaseColor")) _particleMat.SetColor("_BaseColor", Color.white);
        if (_particleMat.HasProperty("_Color")) _particleMat.SetColor("_Color", Color.white);
        if (_particleMat.HasProperty("_UnlitColor")) _particleMat.SetColor("_UnlitColor", Color.white);
        if (_particleMat.HasProperty("_SurfaceType")) _particleMat.SetFloat("_SurfaceType", 1f);
        if (_particleMat.HasProperty("_BlendMode")) _particleMat.SetFloat("_BlendMode", 0f);
        if (_particleMat.HasProperty("_ZWrite")) _particleMat.SetFloat("_ZWrite", 0f);
        if (_particleMat.HasProperty("_ZTest")) _particleMat.SetFloat("_ZTest", 4f);
        if (_particleMat.HasProperty("_Cull")) _particleMat.SetFloat("_Cull", 0f);
        _particleMat.renderQueue = 3000;
        return _particleMat;
    }

    public static Material Mat(Color c)
    {
        int key = (Mathf.RoundToInt(c.r * 255f) << 24)
                | (Mathf.RoundToInt(c.g * 255f) << 16)
                | (Mathf.RoundToInt(c.b * 255f) << 8)
                | Mathf.RoundToInt(c.a * 255f);
        if (_matCache.TryGetValue(key, out var cached) && cached != null) return cached;
        Shader shader = Shader.Find("HDRP/Unlit");
        if (shader == null) shader = Shader.Find("Unlit/Color");
        if (shader == null) shader = Shader.Find("Sprites/Default");
        if (shader == null) shader = Shader.Find("Standard");

        var m = new Material(shader);
        m.name = "VirusMat_" + key.ToString("X8");
        if (m.HasProperty("_UnlitColor")) m.SetColor("_UnlitColor", c);
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
        if (m.HasProperty("_Color")) m.SetColor("_Color", c);
        if (m.HasProperty("_UnlitColorMap")) m.SetTexture("_UnlitColorMap", Texture2D.whiteTexture);
        if (m.HasProperty("_BaseColorMap")) m.SetTexture("_BaseColorMap", Texture2D.whiteTexture);
        if (m.HasProperty("_AlphaCutoffEnable")) m.SetFloat("_AlphaCutoffEnable", 0f);
        if (m.HasProperty("_DoubleSidedEnable")) m.SetFloat("_DoubleSidedEnable", 1f);
        _matCache[key] = m;
        return m;
    }
}

// =====================================================================
//  程序化音效生成器
// =====================================================================
static class SfxGen
{
    const int SR = 22050;
    static readonly Dictionary<string, AudioClip> _cache = new Dictionary<string, AudioClip>();
    static readonly System.Random _rng = new System.Random();

    public static AudioClip Shoot() { return Get("shoot", 0.085f, ShootWave); }
    public static AudioClip EnemyShoot() { return Get("eshoot", 0.140f, EnemyShootWave); }
    public static AudioClip Bounce() { return Get("bounce", 0.070f, BounceWave); }
    public static AudioClip Hit() { return Get("hit", 0.080f, HitWave); }
    public static AudioClip Kill() { return Get("kill", 0.280f, KillWave); }
    public static AudioClip Pickup() { return Get("pickup", 0.110f, PickupWave); }
    public static AudioClip Hurt() { return Get("hurt", 0.200f, HurtWave); }
    public static AudioClip LevelUp() { return Get("levelup", 0.400f, LevelUpWave); }
    public static AudioClip Dash() { return Get("dash", 0.160f, DashWave); }
    public static AudioClip Charge() { return Get("charge", 0.200f, ChargeWave); }
    public static AudioClip Combo() { return Get("combo", 0.100f, ComboWave); }

    static AudioClip Get(string key, float dur, Func<float, float, float> wave)
    {
        if (_cache.TryGetValue(key, out var c) && c != null) return c;
        var clip = MakeClip(key, dur, wave);
        _cache[key] = clip;
        return clip;
    }

    static AudioClip MakeClip(string name, float dur, Func<float, float, float> wave)
    {
        int samples = Mathf.Max(1, Mathf.RoundToInt(dur * SR));
        var data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / SR;
            data[i] = Mathf.Clamp(wave(t, dur), -1f, 1f);
        }
        var clip = AudioClip.Create(name, samples, 1, SR, false);
        clip.SetData(data, 0);
        return clip;
    }

    static float SweepPhase(float t, float f0, float f1, float dur)
    {
        if (dur < 1e-5f) dur = 1e-5f;
        return 2f * Mathf.PI * (f0 * t + (f1 - f0) / (2f * dur) * t * t);
    }

    static float Noise() { return (float)(_rng.NextDouble() * 2.0 - 1.0); }

    static float ShootWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 55f);
        float s = Mathf.Sin(SweepPhase(t, 1800f, 400f, 0.08f));
        return (s * 0.85f + Noise() * 0.15f) * env * 0.9f;
    }

    static float EnemyShootWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 22f);
        float s = Mathf.Sin(SweepPhase(t, 380f, 180f, 0.14f));
        return (s * 0.8f + Noise() * 0.2f) * env * 0.7f;
    }

    static float BounceWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 65f);
        float s = Mathf.Sin(2f * Mathf.PI * 2200f * t);
        float h = Mathf.Sin(2f * Mathf.PI * 3300f * t) * 0.4f;
        return (s + h) * env * 0.75f;
    }

    static float HitWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 40f);
        float s = Mathf.Sin(SweepPhase(t, 700f, 220f, 0.08f)) * 0.5f;
        return (Noise() * 0.6f + s) * env * 0.9f;
    }

    static float KillWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 9f);
        float s = Mathf.Sin(SweepPhase(t, 600f, 70f, 0.28f));
        return (s * 0.7f + Noise() * 0.35f) * env;
    }

    static float PickupWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 18f);
        return Mathf.Sin(SweepPhase(t, 800f, 1500f, 0.11f)) * env * 0.7f;
    }

    static float HurtWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 10f);
        float s = Mathf.Sin(SweepPhase(t, 220f, 110f, 0.2f)) * 0.4f;
        return (Noise() * 0.7f + s) * env * 0.85f;
    }

    static float LevelUpWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 4f);
        float f = 500f + t * 3500f;
        float s1 = Mathf.Sin(2f * Mathf.PI * f * t);
        float s2 = Mathf.Sin(2f * Mathf.PI * (f * 1.25f) * t) * 0.5f;
        float s3 = Mathf.Sin(2f * Mathf.PI * (f * 1.5f) * t) * 0.35f;
        return (s1 + s2 + s3) * env * 0.4f;
    }

    static float DashWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 25f);
        float s = Mathf.Sin(SweepPhase(t, 400f, 1600f, 0.16f));
        return (s * 0.7f + Noise() * 0.15f) * env;
    }

    static float ChargeWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 8f);
        float f = 300f + t * 800f;
        return Mathf.Sin(2f * Mathf.PI * f * t) * env * 0.5f;
    }

    static float ComboWave(float t, float dur)
    {
        float env = Mathf.Exp(-t * 30f);
        return Mathf.Sin(2f * Mathf.PI * 1400f * t) * env * 0.6f;
    }
}