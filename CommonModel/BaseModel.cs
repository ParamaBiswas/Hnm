using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CommonModel
{
    [Serializable]
    public class BaseModel
    {
        #region Private Members
        private bool _IsNew = true;
        private bool _IsDirty = false;
        private bool _IsDeleted = false;
        private string _UserCode;
        private string _ActionType;
        private string _ActionDate;
        private int _CompanyCode;
        #endregion

        #region IModelBase Members

        public virtual bool IsNew
        {
            get { return this._IsNew; }
            set { _IsNew = value; }
        }

        public virtual bool IsDeleted
        {
            get { return this._IsDeleted; }
            set { _IsDeleted = value; }
        }

        public virtual string UserCode
        {
            get { return this._UserCode; }
            set { _UserCode = value; }
        }

        public virtual string ActionType
        {
            get { return this._ActionType; }
            set { _ActionType = value; }
        }

        public virtual string ActionDate
        {
            get { return this._ActionDate; }
            set { _ActionDate = value; }
        }

        public virtual int CompanyCode_FK
        {
            get { return this._CompanyCode; }
            set { _CompanyCode = value; }
        }
        public virtual bool IsDirty
        {
            get { return this._IsDirty; }
            set { _IsDirty = value; }
        }
        #endregion

        protected void MarkOld()
        {
            this.IsNew = false;
        }
        protected void MarkDelete()
        {
            this.IsDeleted = true;
        }
        public BaseModel DeepClone()
        {
            object objResult = null;
            using (MemoryStream ms = new System.IO.MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, this);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult as BaseModel;
        }

    }
}
