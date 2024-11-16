using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlideshow : MonoBehaviour
{
    public string folderPath; // Путь к папке с изображениями
    public RawImage displayImage; // UI элемент RawImage для отображения изображений
    private string[] imageFiles;
    private int currentIndex = 0;

    void Start()
    {
        // Получаем все файлы изображений из указанной папки
        imageFiles = Directory.GetFiles(folderPath, "*.png"); // или "*.jpg" для JPEG
        StartCoroutine(SlideShow());
    }

    private IEnumerator SlideShow()
    {
        while (true)
        {
            if (imageFiles.Length > 0)
            {
                // Загружаем текстуру из файла
                byte[] fileData = File.ReadAllBytes(imageFiles[currentIndex]);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData); // Загружаем изображение в текстуру

                // Применяем текстуру к UI элементу RawImage
                displayImage.texture = texture;

                // Переходим к следующему изображению
                currentIndex = (currentIndex + 1) % imageFiles.Length;
            }

            // Ждем 5 секунд перед показом следующего изображения
            yield return new WaitForSeconds(5f);
        }
    }
}