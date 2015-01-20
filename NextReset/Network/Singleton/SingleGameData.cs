﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Network.Singleton
{
    public class SingleGameData
    {
        private static SingleGameData _Instance = null;
        private bool _UpToDate = false;
        private int[] _AvailableMethods;
        private int[][] _Landscape;
        private String _Name;
        private SingleGameData() { }
        public static SingleGameData Get
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SingleGameData();
                return _Instance;
            }
        }
        public void SetAvailableMethods(int[] availablemethods)
        {
            if (availablemethods == null)
                throw new ArgumentNullException("The array of available methods is NULL");
            if (availablemethods.Length < AppSettings._MinimumAmountOfAvailableMethods)
                throw new FormatException("The array of available methods is to small");
            this._AvailableMethods = availablemethods;
        }
        public void SetLandscape(int[][] tiles)
        {
            if (tiles == null)
                throw new ArgumentNullException("The tiles of the landscape are NULL");
            if (Count(tiles) < 9)
                throw new FormatException("The array of tiles for the landscape is lower then 9. This is to low for a usefull landscape");
            this._Landscape = tiles;
        }
        public void SetLevelName(string name)
        {
            if (name == null || name == "")
                throw new ArgumentNullException("The name of the level is unknown");
            this._Name = name;
        }
        private int Count(int[][] tiles)
        {
            int amount = 0;
            for (int i = 0; i < tiles.Length; i++)
                for (int j = 0; j < tiles[i].Length; j++)
                    amount++;
            return amount;
        }
        public int[] GetAvailableMethods { get { return _AvailableMethods; } }
        public int[][] GetLandscape { get { return _Landscape; } }
        public String GetLevelName { get { return _Name; } }
    }
}