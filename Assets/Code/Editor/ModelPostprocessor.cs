using UnityEditor;

public class ModelPostprocessor : AssetPostprocessor
{
    // What scale to use (1.0 = same as Maya)
    const float scale = 1.0f;

    void OnPreprocessModel()
    {
        ModelImporter importer = assetImporter as ModelImporter;

        importer.animationType = ModelImporterAnimationType.None;
        importer.generateAnimations = ModelImporterGenerateAnimations.None;
        importer.importAnimation = false;
        importer.importMaterials = false;

        importer.useFileScale = false;
        importer.globalScale = scale;
    }


    private static bool HasExtension(string asset, string extension)
    {
        // If asset ends with extension, doesn't care about lower/uppercase
        return asset.EndsWith(extension, System.StringComparison.OrdinalIgnoreCase);
    }
}
