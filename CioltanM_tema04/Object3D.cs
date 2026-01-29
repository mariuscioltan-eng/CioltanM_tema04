using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CioltanM_tema04
{
    class Object3D
    {
        private bool visibility;
        private Randomizer rnd;

        private float sizeX; 
        private float sizeY; 
        private float sizeZ; 

        private float halfX;
        private float halfY;
        private float halfZ;

        private float baseX;
        private float baseY;
        private float baseZ;

        private float fallSpeed;

        private Color color;

        private List<Vector3> loadedFromFile;

        public Object3D()
        {
            rnd = new Randomizer();
            visibility = true;

            loadedFromFile = new List<Vector3>();

            sizeX = rnd.RandomFloat(8f, 15f);   
            sizeY = rnd.RandomFloat(8f, 30f);   
            sizeZ = rnd.RandomFloat(8f, 15f);   

            halfX = sizeX / 2.0f;
            halfY = sizeY / 2.0f;
            halfZ = sizeZ / 2.0f;

            baseX = rnd.RandomFloat(0f, 50f);   
            baseZ = rnd.RandomFloat(0f, 50f);   

            baseY = rnd.RandomFloat(80f, 150f);

            color = rnd.RandomColor();

            fallSpeed = rnd.RandomFloat(1.0f, 3.0f);

            LoadVerticesFromFile();
        }

        private void LoadVerticesFromFile()
        {
            string path = Directory.GetCurrentDirectory() + "\\CubeVertex.txt";

            if (!File.Exists(path))
            {
                Console.WriteLine("AVERTISMENT: Nu am găsit CubeVertex.txt, continui procedural.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] data = line.Split(',');
                        if (data.Length < 3)
                            continue;

                        float lx = float.Parse(data[0].Trim());
                        float ly = float.Parse(data[1].Trim());
                        float lz = float.Parse(data[2].Trim());

                        loadedFromFile.Add(new Vector3(lx, ly, lz));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la citirea CubeVertex.txt: " + ex.Message);
            }
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }
        private bool OnGround()
        {
            return (baseY - halfY) <= 0.0f;
        }

        public void FallObject()
        {
            if (!visibility)
                return;

            if (!OnGround())
            {
                baseY -= fallSpeed;
                float bottomY = baseY - halfY;
                if (bottomY < 0.0f)
                {
                    baseY = halfY;
                }
            }
        }
        public void Draw()
        {
            if (!visibility)
                return;
            Vector3 p000 = new Vector3(baseX - halfX, baseY - halfY, baseZ - halfZ);
            Vector3 p001 = new Vector3(baseX - halfX, baseY - halfY, baseZ + halfZ);
            Vector3 p010 = new Vector3(baseX - halfX, baseY + halfY, baseZ - halfZ);
            Vector3 p011 = new Vector3(baseX - halfX, baseY + halfY, baseZ + halfZ);
            Vector3 p100 = new Vector3(baseX + halfX, baseY - halfY, baseZ - halfZ);
            Vector3 p101 = new Vector3(baseX + halfX, baseY - halfY, baseZ + halfZ);
            Vector3 p110 = new Vector3(baseX + halfX, baseY + halfY, baseZ - halfZ);
            Vector3 p111 = new Vector3(baseX + halfX, baseY + halfY, baseZ + halfZ);

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(color);

            GL.Vertex3(p100);
            GL.Vertex3(p101);
            GL.Vertex3(p111);
            GL.Vertex3(p110);

            GL.Vertex3(p000);
            GL.Vertex3(p010);
            GL.Vertex3(p011);
            GL.Vertex3(p001);

            GL.Vertex3(p010);
            GL.Vertex3(p110);
            GL.Vertex3(p111);
            GL.Vertex3(p011);

            GL.Vertex3(p000);
            GL.Vertex3(p001);
            GL.Vertex3(p101);
            GL.Vertex3(p100);

            GL.Vertex3(p001);
            GL.Vertex3(p011);
            GL.Vertex3(p111);
            GL.Vertex3(p101);

            GL.Vertex3(p000);
            GL.Vertex3(p100);
            GL.Vertex3(p110);
            GL.Vertex3(p010);

            GL.End();
        }
    }
}
