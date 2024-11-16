using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlideshow : MonoBehaviour
{
    public string folderPath; // ���� � ����� � �������������
    public RawImage displayImage; // UI ������� RawImage ��� ����������� �����������
    private string[] imageFiles;
    private int currentIndex = 0;

    void Start()
    {
        // �������� ��� ����� ����������� �� ��������� �����
        imageFiles = Directory.GetFiles(folderPath, "*.png"); // ��� "*.jpg" ��� JPEG
        StartCoroutine(SlideShow());
    }

    private IEnumerator SlideShow()
    {
        while (true)
        {
            if (imageFiles.Length > 0)
            {
                // ��������� �������� �� �����
                byte[] fileData = File.ReadAllBytes(imageFiles[currentIndex]);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData); // ��������� ����������� � ��������

                // ��������� �������� � UI �������� RawImage
                displayImage.texture = texture;

                // ��������� � ���������� �����������
                currentIndex = (currentIndex + 1) % imageFiles.Length;
            }

            // ���� 5 ������ ����� ������� ���������� �����������
            yield return new WaitForSeconds(5f);
        }
    }
}