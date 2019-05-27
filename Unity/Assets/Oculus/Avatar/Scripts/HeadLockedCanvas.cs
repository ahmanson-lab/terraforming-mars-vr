using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLockedCanvas : MonoBehaviour
{
    [SerializeField] private float _distance = 2f;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private GameObject _cameraRig;

    private Vector3 _velocity;
    private Quaternion _rotationVelocity;

    private void Update() {
        // Center canvas in front of face

        transform.position = Vector3.SmoothDamp(
            transform.position,
            _cameraRig.transform.position + _cameraRig.transform.rotation * new Vector3(0, 0, _distance),
            ref _velocity,
            _smoothTime); 

        transform.rotation = SmoothDampQuaternion(
            transform.rotation,
            _cameraRig.transform.rotation,
            ref _rotationVelocity,
            _smoothTime);
    }

    // From https://gist.github.com/maxattack/4c7b4de00f5c1b95a33b
    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target,
        ref Quaternion currentVelocity, float smoothTime) {
        // account for double-cover
        var dot = Quaternion.Dot(current, target);
        var multi = dot > 0f ? 1f : -1f;
        target.x *= multi;
        target.y *= multi;
        target.z *= multi;
        target.w *= multi;
        // smooth damp (nlerp approx)
        var result = new Vector4(
            Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime),
            Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime),
            Mathf.SmoothDamp(current.w, target.w, ref currentVelocity.w, smoothTime)
        ).normalized;
        // compute deriv
        var dtInv = 1f / Time.deltaTime;
        currentVelocity.x = (result.x - current.x) * dtInv;
        currentVelocity.y = (result.y - current.y) * dtInv;
        currentVelocity.z = (result.z - current.z) * dtInv;
        currentVelocity.w = (result.w - current.w) * dtInv;
        return new Quaternion(result.x, result.y, result.z, result.w);
    }
 
}