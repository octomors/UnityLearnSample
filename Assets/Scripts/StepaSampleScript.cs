using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class StepaSampleScript : SampleScript
{
    [SerializeField]
    [Min(0)]
    private Vector3 rotationDelta = new Vector3(0, 90, 0);

    [SerializeField]
    [Min(0.1f)]
    private float rotationSpeed = 10f;

    public override void Use()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        sus();

        Quaternion startRot = transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(rotationDelta);

        float angle = Quaternion.Angle(startRot, targetRot);
        float duration = angle / rotationSpeed;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }

    #region  sus

    [SerializeField] private AudioClip rotationSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

    }

    private void sus()
    {
        audioSource.clip = rotationSound;
        audioSource.Play();

        Invoke(nameof(StopSound), 2f);
    }

    private void StopSound() => audioSource.Stop();
    #endregion
}
