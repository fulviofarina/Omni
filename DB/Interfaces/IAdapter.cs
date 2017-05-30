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
   


   

        // string AppPath { get; set; } void Help();

        // void PopulateUserDirectories();

     

        // bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref
        // DB.LINAA.TAMDeleteMethod remover);
        TableAdapterManager TAM { get; set; }

        bool IsMainConnectionOk { get; }
        string Exception { get; }
     
        void SetConnections(/*string localDB, string developerDB, */ string defaultConnection);
    }
}