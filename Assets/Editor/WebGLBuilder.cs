using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DungeonEclipse.EditorTools
{
    /// <summary>
    /// Gera o build WebGL do jogo a partir das cenas configuradas em Build Settings.
    /// Pode ser executado pela linha de comando em batch mode:
    ///   Unity.exe -quit -batchmode -projectPath . -executeMethod DungeonEclipse.EditorTools.WebGLBuilder.Build
    /// O caminho de saida pode ser passado com -buildOutput &lt;pasta&gt;.
    /// </summary>
    public static class WebGLBuilder
    {
        private const string DefaultOutput = "Builds/WebGL";

        [MenuItem("Build/WebGL Build")]
        public static void Build()
        {
            string output = ResolveOutputPath();

            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("[WebGLBuilder] Nenhuma cena habilitada em Build Settings.");
                EditorApplication.Exit(1);
                return;
            }

            // Compressao Brotli gera arquivos menores; itch.io serve com headers corretos.
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.WebGL.decompressionFallback = true;

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            Debug.Log($"[WebGLBuilder] Iniciando build WebGL com {scenes.Length} cenas em '{output}'.");
            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log($"[WebGLBuilder] Build concluido: {summary.totalSize} bytes em '{output}'.");
            }
            else
            {
                Debug.LogError($"[WebGLBuilder] Build falhou: {summary.result} ({summary.totalErrors} erros).");
                if (Application.isBatchMode)
                    EditorApplication.Exit(1);
            }
        }

        private static string ResolveOutputPath()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "-buildOutput")
                    return args[i + 1];
            }
            return DefaultOutput;
        }
    }
}
