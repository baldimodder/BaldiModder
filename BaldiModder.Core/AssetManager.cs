using System;
using System.Collections.Generic;
using System.IO;

using BaldiModder.Data;
using BaldiModder.Data.IO;

using UnityEngine;

using Random = System.Random;

namespace BaldiModder {
    public class AssetManager {

        public const string configFilePath = "config";
        public const string modInfoPath = "mod";

        public const string audioFileRepositoryPath = "repos/audio";
        public const string imageFileRepositoryPath = "repos/image";
        public const string animationFileRepositoryPath = "repos/animation";

        public const string soundReplacementsPath = "replace/sound";
        public const string textureReplacementsPath = "replace/textures";
        public const string spriteReplacementsPath = "replace/sprites";

        public static IDataSerializer DataSerializer { get; set; }

        public static string FilePath { get; set; }

        public static Config Config { get; set; }

        public static ModInfo ModInfo { get; set; }

        public static AudioFileRepository AudioRepository { get; set; }
        public static ImageFileRepository ImageRepository { get; set; }
        public static AnimationFileRepository AnimationRepository { get; set; }

        public static Dictionary<string, AudioClip> LoadedAudioFiles { get; set; }
        public static Dictionary<string, Texture2D> LoadedTextures { get; set; }
        public static Dictionary<string, Sprite> LoadedSprites { get; set; }
        public static Dictionary<string, Data.Animation> LoadedAnimations { get; set; }

        public static Dictionary<string, SoundReplacement> SoundReplacements { get; set; }
        public static Dictionary<string, TextureReplacement> TextureReplacements { get; set; }
        public static Dictionary<string, SpriteReplacement> SpriteReplacements { get; set; }

        static AssetManager() {
            DataSerializer = new JsonDataSerializer();

            LoadedAudioFiles = new Dictionary<string, AudioClip>();
            LoadedTextures = new Dictionary<string, Texture2D>();
            LoadedSprites = new Dictionary<string, Sprite>();
            LoadedAnimations = new Dictionary<string, Data.Animation>();
        }


        /// <summary>
        /// Checks the sound replacements.
        /// </summary>
        public static void CheckSoundReplacements() {
            if (!DataExists(soundReplacementsPath)) {
                SaveData(soundReplacementsPath, new Dictionary<string, SoundReplacement> {
                    { "BAL_Slap", new SoundReplacement {
                        SoundNames = new List<string> {
                            "slap"
                        },
                        ReplacementMode = SoundReplacementMode.PickFirstSound
                    } }
                });
            } else {
                SoundReplacements = ReadData<Dictionary<string, SoundReplacement>>(soundReplacementsPath);
            }
        }

        /// <summary>
        /// Checks the image replacements.
        /// </summary>
        public static void CheckTextureReplacements() {
            if (!DataExists(textureReplacementsPath)) {
                SaveData(textureReplacementsPath, new Dictionary<string, TextureReplacement> {
                    { "win", new TextureReplacement {
                        Name = "you_win"
                    } }
                });
            } else {
                TextureReplacements = ReadData<Dictionary<string, TextureReplacement>>(textureReplacementsPath);
            }
        }

        /// <summary>
        /// Checks the image replacements.
        /// </summary>
        public static void CheckSpriteReplacements() {
            if (!DataExists(spriteReplacementsPath)) {
                SaveData(spriteReplacementsPath, new Dictionary<string, SpriteReplacement> {
                    { "win", new SpriteReplacement {
                        Name = "you_win"
                    } }
                });
            } else {
                SpriteReplacements = ReadData<Dictionary<string, SpriteReplacement>>(spriteReplacementsPath);
            }
        }

        /// <summary>
        /// Ensures a directory exists.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void EnsureDirectoryExists(string directory) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Ensures a file's directory exists.
        /// </summary>
        /// <param name="path">The file path.</param>
        public static void EnsureFileDirectoryExists(string path) {
            EnsureDirectoryExists(Path.GetDirectoryName(path));
        }

        /// <summary>
        /// Gets a path in the mod folder.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A string.</returns>
        public static string GetPathInModFolder(string path) {
            return Path.Combine(FilePath, path);
        }

        /// <summary>
        /// If a file exists.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>A boolean.</returns>
        public static bool FileExists(string path, bool inModFolder = true) {
            return File.Exists(inModFolder ? GetPathInModFolder(path) : path);
        }

        /// <summary>
        /// Saves JSON data to a file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="obj">The object to save.</param>
        public static void SaveData(string path, object obj, bool inModFolder = true, bool saveType = false) {
            if (inModFolder) path = GetPathInModFolder(path);
            try {
                EnsureFileDirectoryExists(path);
            } catch { }

            DataSerializer.SaveData(path, obj, saveType: saveType);
        }

        /// <summary>
        /// Reads JSON data from a file.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns>The specified type.</returns>
        public static T ReadData<T>(string path, bool inModFolder = true, bool saveType = false) {
            if (inModFolder) path = GetPathInModFolder(path);

            return DataSerializer.ReadData<T>(path, saveType: saveType);
        }

        public static bool DataExists(string path, bool inModFolder = true) {
            if (inModFolder) path = GetPathInModFolder(path);
            return DataSerializer.DataExists(path);
        }

        public static void UpdateCursor() {
            if (Config.Cursor.UseSystemCursor) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            else Cursor.SetCursor(GetTexture(Config.Cursor.CursorImage), Config.Cursor.Pivot.AsVector2, Config.Cursor.UseHardwareCursorIfSupported ? CursorMode.Auto : CursorMode.ForceSoftware);
        }

        /// <summary>
        /// Gets and audio clip from the audio repository.
        /// </summary>
        /// <param name="audio">The name of the clip.</param>
        /// <returns>An AudioClip.</returns>
        public static AudioClip GetAudioClip(string audio) {
            if (LoadedAudioFiles.ContainsKey(audio)) return LoadedAudioFiles[audio];
            WWW www = new WWW(AudioRepository.GetPathInModFolder(audio));

            AudioClip clip = www.GetAudioClip();
            LoadedAudioFiles[audio] = clip;

            return clip;
        }

        public static Texture2D GetTexture(string texture) {
            if (LoadedTextures.ContainsKey(texture)) return LoadedTextures[texture];

            Debug.Log($"{texture} is not already loaded.");
            WWW www = new WWW(ImageRepository.GetPathInModFolder(texture));

            //Debug.Log($"{texture} - {www.texture.width.ToString()}x{www.texture.height.ToString()}");
            Texture2D tex = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            tex.filterMode = ImageRepository.Files[texture].FilterMode;

            www.LoadImageIntoTexture(tex);
            LoadedTextures[texture] = tex;

            return tex;
        }

        public static Texture2D ReplaceTexture(Texture2D original) {
            if (original == null) {
                //Debug.Log("original is null");
                return original;
            }

            if (!(TextureReplacements.ContainsKey(original.name))) return original;

            Debug.Log($"Replacing {original.name} with {TextureReplacements[original.name].Name}");

            return GetTexture(TextureReplacements[original.name].Name);
        }

        public static Sprite GetSprite(string sprite, Vector2 pivot, float pixelsPerUnit = 0f) {
            string dictionaryEntry = $"{sprite},{pivot.x.ToString()},{pivot.y.ToString()}";

            if (LoadedSprites.ContainsKey(dictionaryEntry)) {
                return LoadedSprites[dictionaryEntry];
            }

            Texture2D baseTexture = GetTexture(sprite);
            Sprite newSprite = Sprite.Create(baseTexture, new Rect(0, 0, baseTexture.width, baseTexture.height), pivot, pixelsPerUnit, 0, SpriteMeshType.FullRect);

            LoadedSprites[dictionaryEntry] = newSprite;
            return newSprite;
        }

        public static float GeneratePixelsPerUnit(float oldPixelsPerUnit, Vector2 oldSpriteScale, Vector2 newSpriteScale) {
            if (oldSpriteScale.x == newSpriteScale.x && oldSpriteScale.y == newSpriteScale.y) return oldPixelsPerUnit;

            float width = oldSpriteScale.x / oldPixelsPerUnit;
            return oldSpriteScale.x * oldPixelsPerUnit;
        }

        public static Sprite ReplaceSprite(Sprite original) {
            if (!(SpriteReplacements.ContainsKey(original.name))) return original;

            Texture2D tex = GetTexture(SpriteReplacements[original.name].Name);

            return GetSprite(SpriteReplacements[original.name].Name, /*new Vector2(0, 0)*/ original.pivot / original.textureRect.size, GeneratePixelsPerUnit(original.pixelsPerUnit, original.rect.size, new Vector2(tex.width, tex.height)));
        }

        public static Vector2 GetSpritePivot(Sprite original) {
            return original.pivot / original.textureRect.size;
        }

        public static AudioClip ReplaceAudioClip(AudioClip original) {
            return ReplaceAudioClip(original, SoundReplacementMode.PickFirstSound, 0);
        }

        public static AudioClip ReplaceAudioClip(AudioClip original, int number) {
            return ReplaceAudioClip(original, SoundReplacementMode.SpecificSoundOrder, number);
        }

        public static AudioClip ReplaceAudioClip(AudioClip original, SoundReplacementMode mode, int number) {
            //TODO: Reimplement this.
            //if (BaldisBasics.VersionData.NonReplaceableAudioFiles.ContainsKey(original.name)) return original;

            try {
                if (!SoundReplacements.ContainsKey(original.name)) return original; //Return the original if it's not in the sound replacements.
                SoundReplacement soundReplacement = SoundReplacements[original.name];

                switch (mode) {
                    case SoundReplacementMode.PickFirstSound:
                        return GetAudioClip(soundReplacement.SoundNames[0]);
                    case SoundReplacementMode.PickRandomSound:
                        return GetAudioClip(soundReplacement.SoundNames[new Random(DateTime.Now.Millisecond).Next(soundReplacement.SoundNames.Count - 1)]);
                    case SoundReplacementMode.SpecificSoundOrder:
                        return GetAudioClip(soundReplacement.SoundNames[number]);
                    default:
                        return GetAudioClip(soundReplacement.SoundNames[0]);

                }
            } catch { }

            return original;
        }

        public static Data.Animation GetAnimation(string name) {
            return LoadedAnimations[name];
        }
    }
}
