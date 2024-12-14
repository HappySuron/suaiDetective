using UnityEngine;
using UnityEngine.Audio; // Для работы с AudioMixer
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Синглтон для глобального доступа

    [Header("Audio Sources")]
    public AudioSource musicSource;   // Источник для фоновой музыки
    public AudioSource sfxSource;     // Источник для SFX (2D)
    public AudioSource dialogueSource; // Источник для диалогов (2D)

    [Header("Audio Mixer Settings")]
    public AudioMixerGroup musicGroup;   // Группа для музыки
    public AudioMixerGroup sfxGroup;     // Группа для SFX
    public AudioMixerGroup dialogueGroup; // Группа для диалогов

    [Header("Background Music")]
    public AudioClip defaultBackgroundMusic;


    [Header("Subtitles")]
    public TMP_Text subtitleText; // UI-элемент для отображения субтитров
    public float subtitleFadeDuration = 1f; // Время исчезновения субтитров

    private void Awake()
    {
        // Настраиваем синглтон
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
    }

    private void Start()
    {
        // Автозапуск фоновой музыки
        if (defaultBackgroundMusic != null)
        {
            PlayMusic(defaultBackgroundMusic);
        }
    }

    // Воспроизведение музыки (фоновой)
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicGroup; // Применяем группу микшера для музыки
        musicSource.Play();
    }

    // Воспроизведение SFX на NPC (3D звук)
    public void PlaySFXOnObject(GameObject obj, AudioClip sfxClip)
    {
        // Получаем или добавляем AudioSource на NPC
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source == null)
        {
            source = obj.AddComponent<AudioSource>();
        }

        // Устанавливаем параметры для 3D звука
        source.clip = sfxClip;
        source.spatialBlend = 1.0f; // Задаем 3D-звук (не плоский)
        source.outputAudioMixerGroup = sfxGroup; // Применяем группу микшера для SFX
        source.Play();

        // Удаляем AudioSource после проигрывания
        Destroy(source, sfxClip.length);
    }

    public void PlayDialogue(GameObject npc, AudioClip dialogueClip, string subtitles = null)
    {
        if (npc == null)
        {
            Debug.LogWarning("NPC объект не указан. Используйте перегруженный метод для 2D звука.");
            return;
        }

        // Воспроизведение 3D звука на NPC
        AudioSource source = npc.GetComponent<AudioSource>();
        if (source == null)
        {
            source = npc.AddComponent<AudioSource>();
        }

        source.clip = dialogueClip;
        source.spatialBlend = 1.0f; // 3D звук
        source.outputAudioMixerGroup = dialogueGroup;
        source.Play();

        // Отображение субтитров
        if (!string.IsNullOrEmpty(subtitles) && subtitleText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowSubtitles(subtitles, dialogueClip.length));
        }

        Destroy(source, dialogueClip.length);
    }

    public void PlayDialogue(AudioClip dialogueClip, string subtitles = null)
    {
        if (dialogueClip == null)
        {
            Debug.LogWarning("Диалоговый аудиоклип отсутствует.");
            return;
        }

        // Воспроизведение 2D звука через глобальный источник
        dialogueSource.clip = dialogueClip;
        dialogueSource.outputAudioMixerGroup = dialogueGroup;
        dialogueSource.Play();

        // Отображение субтитров
        if (!string.IsNullOrEmpty(subtitles) && subtitleText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowSubtitles(subtitles, dialogueClip.length));
        }
    }


    // Корутин для показа субтитров
    private System.Collections.IEnumerator ShowSubtitles(string text, float duration)
    {
        subtitleText.text = text;
        subtitleText.color = new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, 1);

        yield return new WaitForSeconds(duration);

        // Плавное исчезновение субтитров
        float elapsedTime = 0;
        Color initialColor = subtitleText.color;
        while (elapsedTime < subtitleFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / subtitleFadeDuration);
            subtitleText.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        subtitleText.text = "";
    }
}
