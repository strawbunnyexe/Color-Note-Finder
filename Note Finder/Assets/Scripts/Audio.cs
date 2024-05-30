using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Audio : MonoBehaviour
{
    public SpriteRenderer[] sprites;

    public TextMeshProUGUI noteText;

    public AudioSource audioSource;
    public int sampleSize = 256;
    private float[] spectrumData;

    // Start is called before the first frame update
    void Start()
    {
        //set size of data array
        spectrumData = new float[sampleSize];

        //disable flashing tiles
        for(int i = 0; i< sprites.Length; i++)
        {
            sprites[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            // Get spectrum data of current audio being played
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

            // Find dominant frequency
            float maxFrequency = 0f;
            int maxIndex = 0;
            for (int i = 0; i < sampleSize / 2; i++)
            {
                if (spectrumData[i] > maxFrequency)
                {
                    maxFrequency = spectrumData[i];
                    maxIndex = i;
                }
            }

            // Calculate corresponding musical note (example mapping)
            float frequency = maxIndex * AudioSettings.outputSampleRate / sampleSize;
            string note = FrequencyToNote(frequency);
            Debug.Log("Detected Note: " + note);
            // update note text to display
            noteText.text = "Current Note: " + note;

            // Flash note that is currently being played
            switch (note)
            {
                case "A":
                    sprites[0].enabled = true;
                    //reset other sprites
                    DisableOthers(0);
                    break;
                case "E":
                    sprites[1].enabled = true;
                    //reset other sprites
                    DisableOthers(1);
                    break;
                case "B":
                    sprites[2].enabled = true;
                    //reset other sprites
                    DisableOthers(2);
                    break;
                case "F#":
                    sprites[3].enabled = true;
                    //reset other sprites
                    DisableOthers(3);
                    break;
                case "C#":
                    sprites[4].enabled = true;
                    //reset other sprites
                    DisableOthers(4);
                    break;
                case "G#":
                    sprites[5].enabled = true;
                    //reset other sprites
                    DisableOthers(5);
                    break;
                case "D#":
                    sprites[6].enabled = true;
                    //reset other sprites
                    DisableOthers(6);
                    break;
                case "A#":
                    sprites[7].enabled = true;
                    //reset other sprites
                    DisableOthers(7);
                    break;
                case "F":
                    sprites[8].enabled = true;
                    //reset other sprites
                    DisableOthers(8);
                    break;
                case "C":
                    sprites[9].enabled = true;
                    //reset other sprites
                    DisableOthers(9);
                    break;
                case "G":
                    sprites[10].enabled = true;
                    //reset other sprites
                    DisableOthers(10);
                    break;
                case "D":
                    sprites[11].enabled = true;
                    //reset other sprites
                    DisableOthers(11);
                    break;
            }
        }
    }

    // Helper function to disable active sprites for reset
    private void DisableOthers(int index) 
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i != index)
            {
                sprites[i].enabled = false;
            }
        }
    }

    // Determines the note being played based on the current frequency of audio
    private string FrequencyToNote(float frequency)
    {
        float halfStep = Mathf.Pow(2, 1 / 12f);
        float A4Frequency = 440f; // A4 frequency in Hz
        float halfStepsFromA4 = Mathf.Log(frequency / A4Frequency, halfStep);
        int noteIndex = Mathf.RoundToInt(halfStepsFromA4);
        string[] notes = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

        // Calculate the octave and note index
        int octave = 4 + noteIndex / 12;
        int note = noteIndex % 12;

        // Handle edge cases where note index is out of range
        if (note < 0)
        {
            note += 12;
            octave--;
        }
        else if (note >= notes.Length)
        {
            note -= 12;
            octave++;
        }

        // Ensure note index is within the range of the notes array
        note = Mathf.Clamp(note, 0, notes.Length - 1);

        return notes[note].ToString();
    }
}
