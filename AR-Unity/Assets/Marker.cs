using UnityEngine;
using System.Collections;

// This class moves an object associated with this script, according to the transformation
// matrix get in the background class.
public class Marker : MonoBehaviour {

    // Get a rotation Quaternion from a Matrix
    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
        q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
        q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
        q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));

        // We need to invert rotations on X and Z axis
        q.x = -q.x;
        q.z = -q.z;

        return q;
    }

    // Get a Transform ofject from a Matrix
    public static void TransformFromMatrix(Matrix4x4 matrix, Transform trans)
    {
        //trans.localRotation = QuaternionFromMatrix(matrix);
        trans.rotation = QuaternionFromMatrix(matrix);
        Vector3 tmp = matrix.GetColumn(3); // uses implicit conversion from Vector4 to Vector3
        // We need to invert the translation on the Y axis
        tmp.y = -tmp.y;
        //trans.localPosition = tmp;
        //tmp.z = 250;
        trans.position = tmp;
    }

	// Update is called once per frame
    void Update()
    {
        // We use the transformation matrix of the Background class
        Matrix4x4 mat = new Matrix4x4();
        for (int i = 0; i < 16; ++i)
            mat[i] = (float)Background.transMat[i];

        // We move the object only if the transformation matrix is different
        // from a matrix with all elements equal to zero .
        if (!mat.Equals(Matrix4x4.zero))
            TransformFromMatrix(mat, this.transform);
    }
}
