using System.ComponentModel;
using System.Data.Linq.Mapping;
using System;
using Pro_Salles.DAL;

namespace Pro_Salles.DAL
{
    partial class Pro_SallesDataContext
    {
        partial void InsertInvoice_Return_Detail(Invoice_Return_Detail instance);
        partial void UpdateInvoice_Return_Detail(Invoice_Return_Detail instance);
        partial void DeleteInvoice_Return_Detail(Invoice_Return_Detail instance);

		public System.Data.Linq.Table<Invoice_Return_Detail> Invoice_Return_Details
		{
			get
			{
				return this.GetTable<Invoice_Return_Detail>();
			}
		}
	}





	//101/102
	[global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Invoice_Return_Details")]
	public partial class Invoice_Return_Detail : Invoice_Detail
	{

		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

		private int _ID;

		private int _SourceRowID;

		#region Extensibility Method Definitions
		
		partial void OnCreated();

		partial void OnIDChanging(int value);
		partial void OnIDChanged();
		
		partial void OnSourceRowIDChanging(int value);
		partial void OnSourceRowIDChanged();
		#endregion

		public Invoice_Return_Detail()
		{
			OnCreated();
		}

		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
		//To be able to use the specified ID Column for Invoice_Return_Details use it as new or add keyword (new)
		public new int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}



		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SourceRowID", DbType = "Int NOT NULL")]
		public int SourceRowID
		{
			get
			{
				return this._SourceRowID;
			}
			set
			{
				if ((this._SourceRowID != value))
				{
					this.OnSourceRowIDChanging(value);
					this.SendPropertyChanging();
					this._SourceRowID = value;
					this.SendPropertyChanged("SourceRowID");
					this.OnSourceRowIDChanged();
				}
			}
		}		
	}
}