using System;
using System.Collections.Generic;
using System.Data;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IAdapter
    {
        void InitializeComponent();
   //    void  disposeComponent();
        void DisposeAdapters();

        QTA QTA { get; set; }

        void DisposeSolCoinAdapters();

        void InitializeAdapters();

        void InitializeSolCoinAdapters();

        TableAdapterManager TAM { get; set; }

        bool IsMainConnectionOk { get; }
        string Exception { get; }
    }
}