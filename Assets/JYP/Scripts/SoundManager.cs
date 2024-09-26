using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum ESfxType
    {
        Success,
    }

    public enum EBgmType
    {
        None = -1,
        Lobby = 0,
        Main,
        Dance,
    }

    const string SoundManagerPath = "Prefabs/UX/SoundManager";

    public AudioClip[] bgm;
    public AudioClip[] sfx;

    public AudioSource bgmSource;

    private EBgmType currentBgmType = EBgmType.None;
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var prefab = Resources.Load<GameObject>(SoundManagerPath);
                var go = Instantiate(prefab);
                instance = go.GetComponent<SoundManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Public Methods

    public void PlaySfx(ESfxType type, Vector3 position)
    {
        var index = (int)type;
        if (index < 0 || index >= sfx.Length)
        {
            Debug.LogError("Invalid SFX index");
            return;
        }

        AudioSource.PlayClipAtPoint(sfx[(int)type], position);
    }

    public void PlayBgm(EBgmType type)
    {
        if (type == EBgmType.None) return;
        if (currentBgmType == type) return;
        currentBgmType = type;
        StartCoroutine(PlayBgmCoroutine(type));
    }

    #endregion

    #region Private Methods

    private IEnumerator PlayBgmCoroutine(EBgmType type)
    {
        yield return new WaitUntil(() => bgmSource != null);
        var index = (int)type;
        if (index < 0 || index >= bgm.Length)
        {
            Debug.LogError("Invalid BGM index");
            yield break;
        }

        bgmSource.clip = bgm[index];
        bgmSource.Play();
    }

    #endregion
}