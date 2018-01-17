using UnityEngine;

public class PSTest : MonoBehaviour
{
    public Material material;

    private void OnPostRender()
    {
        GL.PushMatrix();
        material.SetPass(0);
        GL.Color(new Color(1, 1, 1, 1));
        GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
        GL.Begin(GL.QUADS);
        GL.TexCoord(new Vector3(0, 0, 0));
        GL.Vertex3(0.25F, 0.25F, 0);
        GL.TexCoord(new Vector3(0, 1, 0));
        GL.Vertex3(0.25F, 0.75F, 0);
        GL.TexCoord(new Vector3(1, 1, 0));
        GL.Vertex3(0.75F, 0.75F, 0);
        GL.TexCoord(new Vector3(1, 0, 0));
        GL.Vertex3(0.75F, 0.25F, 0);
        GL.End();
        GL.PopMatrix();
    }
}
