using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource dialogueSource;

    [Header("Audio Mixer Settings")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup dialogueGroup;

    [Header("Background Music")]
    public AudioClip defaultBackgroundMusic;

    [Header("Subtitles")]
    public TMP_Text subtitleText;
    public float subtitleFadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (defaultBackgroundMusic != null)
        {
            PlayMusic(defaultBackgroundMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.Play();
    }

    public void PlayDialogueSequence(AudioClip[] dialogueClips, string[] subtitles = null)
    {
        if (dialogueClips == null || dialogueClips.Length == 0)
        {
            Debug.LogWarning("Нет аудиоклипов для воспроизведения.");
            return;
        }

        if (subtitles != null && subtitles.Length != dialogueClips.Length)
        {
            Debug.LogWarning("Количество субтитров не совпадает с количеством аудиоклипов.");
            return;
        }

        StartCoroutine(PlayDialogueQueue(dialogueClips, subtitles));
    }

    private IEnumerator PlayDialogueQueue(AudioClip[] clips, string[] subtitles)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            AudioClip clip = clips[i];
            string subtitle = subtitles != null && i < subtitles.Length ? subtitles[i] : null;

            PlayDialogue(clip, subtitle);

            // Ждем, пока аудио проигрывается
            yield return new WaitForSeconds(clip.length);

            // Добавим небольшую паузу между диалогами (если нужно)
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PlayDialogue(AudioClip dialogueClip, string subtitles = null)
    {
        if (dialogueClip == null)
        {
            Debug.LogWarning("Диалоговый аудиоклип отсутствует.");
            return;
        }

        dialogueSource.clip = dialogueClip;
        dialogueSource.outputAudioMixerGroup = dialogueGroup;
        dialogueSource.Play();

        if (!string.IsNullOrEmpty(subtitles) && subtitleText != null)
        {
            // Используем длину аудиофайла для определения времени показа субтитров
            StartCoroutine(ShowSubtitles(subtitles, dialogueClip.length));
        }
    }

    private IEnumerator ShowSubtitles(string text, float duration)
    {
        subtitleText.text = text;
        subtitleText.color = new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, 1);

        // Ждем, пока длительность аудио
        yield return new WaitForSeconds(duration);

        // Начинаем исчезновение субтитров
        float elapsedTime = 0;
        Color initialColor = subtitleText.color;
        while (elapsedTime < subtitleFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / subtitleFadeDuration);
            subtitleText.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        subtitleText.text = ""; // Очищаем субтитры после исчезновения
    }
}
