using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stoneClips;
    [SerializeField]
    private AudioClip[] mudClips;
    [SerializeField]
    private AudioClip[] grassClips;

    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    private float stepCooldown = 0.2f; // Èas mezi kroky
    private float nextStepTime = 0f; // Èas, kdy se mùže pøehrát další krok

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        terrainDetector = new TerrainDetector();
    }

    public void PlayStep()
    {
        if (Time.time >= nextStepTime)
        {
            nextStepTime = Time.time + stepCooldown;
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
        }
    }

    private AudioClip GetRandomClip()
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch (terrainTextureIndex)
        {
            case 0:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            case 1:
                return mudClips[UnityEngine.Random.Range(0, mudClips.Length)];
            case 2:
                return stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
            case 3:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            case 4:
                return stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
            case 5:
            default:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
        }

    }
}
