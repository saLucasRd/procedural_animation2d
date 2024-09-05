using UnityEngine;

public class FootPositioner : MonoBehaviour {
    public GameObject playerObj;

    public Transform target;   
 
    public FootPositioner otherFoot;    

    public bool isBalanced;

    public float lerp;

    private Vector3 startPos;
    private Vector3 endPos;

    public float overShootFactor = 0.8f;
    public float stepSpeed = 3f; 
    public float footDisplacementOnX = 0.25f;

    private void Start() {
    startPos = endPos = target.position;
}

    private void Update() {
        bool thisFootCanMove = otherFoot.lerp > 1 && lerp > otherFoot.lerp;
        if (!isBalanced && lerp > 1 && thisFootCanMove) {
            CalculateNewStep();
        }
        float easedLerp = EaseInOutCubic(lerp);
        target.position = Vector3.Lerp(startPos, endPos, easedLerp);
        lerp += Time.deltaTime * stepSpeed;
        UpdateBalance();
        
    }

    private void UpdateBalance() {
        float centerOfMass = playerObj.transform.position.x;    
        isBalanced = IsFloatInRange(centerOfMass, target.position.x, otherFoot.target.position.x);      
    }

    bool IsFloatInRange(float value, float bound1, float bound2) {
        float minValue = Mathf.Min(bound1, bound2);
        float maxValue = Mathf.Max(bound1, bound2);
        return value > minValue && value < maxValue;
    }

    private float EaseInOutCubic(float x) {
        return 1f / (1 + Mathf.Exp(-10 * (x - 0.5f)));
    }
    private void CalculateNewStep() {
        startPos = target.position;
        lerp = 0;
        RaycastHit2D ray = Physics2D.Raycast(playerObj.transform.position + new Vector3(footDisplacementOnX, 0, 0), Vector2.down, 10);

        Vector3 posDiff = ((Vector3)ray.point - target.position) * (1 + overShootFactor);

        endPos = target.position + posDiff;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position, 0.1f);
    }

}