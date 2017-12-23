using System;
using UnityEngine;

/// <summary>
/// Serializable vector3, used for serialization of the position of Unity postions,
/// which cannot be serialied
/// </summary>
[Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>The x.</value>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>The y.</value>
    public float Y { get; set; }

    /// <summary>
    /// Gets or sets the z.
    /// </summary>
    /// <value>The z.</value>
    public float Z { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SerializableVector3"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    public SerializableVector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SerializableVector3"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public SerializableVector3(float x, float y) : this(x, y, 0)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SerializableVector3"/> struct.
    /// </summary>
    /// <param name="unityVector3">Unity vector3.</param>
    public SerializableVector3(Vector3 unityVector3) : this(unityVector3.x, unityVector3.y, unityVector3.z)
    {
        
    }

    public override string ToString()
    {
        return string.Format("[SerializableVector3: X={0}, Y={1}, Z={2}]", X, Y, Z);
    }

    /// <summary>
    /// Tos the vector3.
    /// </summary>
    /// <returns>The vector3.</returns>
    public Vector3 ToVector3() 
    {
        return new Vector3(this.X, this.Y, this.Z);
    }

}

[Serializable]
public struct SerializableQuaternion 
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public SerializableQuaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public SerializableQuaternion(Quaternion quaternion)
    {
        X = quaternion.x;
        Y = quaternion.y;
        Z = quaternion.z;
        W = quaternion.w;
    }

    public Quaternion ToQuaternion() 
    {
        return new Quaternion(X, Y, Z, W);
    }
}