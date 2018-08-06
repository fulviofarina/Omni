using DB.LINAATableAdapters;

namespace DB
{
    public interface IAdapter
    {
        void SetHyperLabConnection(string defaultConnection);

        void InitializeComponent();

        void DisposePeaksAdapters();

        // void disposeComponent();
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
        bool IsHyperLabConnectionOk { get; }

        void SetMainConnection(/*string localDB, string developerDB, */ string defaultConnection);

        void InitializePeaksAdapters(bool forHyperlab);
    }
}