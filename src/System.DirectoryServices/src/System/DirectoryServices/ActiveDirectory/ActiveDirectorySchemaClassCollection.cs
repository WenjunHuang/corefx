// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.DirectoryServices.ActiveDirectory
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public class ActiveDirectorySchemaClassCollection : CollectionBase
    {
        private DirectoryEntry _classEntry = null;
        private string _propertyName = null;
        private ActiveDirectorySchemaClass _schemaClass = null;
        private bool _isBound = false;
        private DirectoryContext _context = null;

        internal ActiveDirectorySchemaClassCollection(DirectoryContext context,
                                                        ActiveDirectorySchemaClass schemaClass,
                                                        bool isBound,
                                                        string propertyName,
                                                        ICollection classNames,
                                                        bool onlyNames)
        {
            _schemaClass = schemaClass;
            _propertyName = propertyName;
            _isBound = isBound;
            _context = context;

            foreach (string ldapDisplayName in classNames)
            {
                // all properties in writeable class collection are non-defunct
                // so calling constructor for non-defunct class
                this.InnerList.Add(new ActiveDirectorySchemaClass(context, ldapDisplayName, (DirectoryEntry)null, null));
            }
        }

        internal ActiveDirectorySchemaClassCollection(DirectoryContext context,
                                                        ActiveDirectorySchemaClass schemaClass,
                                                        bool isBound,
                                                        string propertyName,
                                                        ICollection classes)
        {
            _schemaClass = schemaClass;
            _propertyName = propertyName;
            _isBound = isBound;
            _context = context;

            foreach (ActiveDirectorySchemaClass schClass in classes)
            {
                this.InnerList.Add(schClass);
            }
        }

        public ActiveDirectorySchemaClass this[int index]
        {
            get
            {
                return (ActiveDirectorySchemaClass)List[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (!value.isBound)
                {
                    throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, value.Name));
                }

                if (!Contains(value))
                {
                    List[index] = value;
                }
                else
                {
                    throw new ArgumentException(Res.GetString(Res.AlreadyExistingInCollection, value), "value");
                }
            }
        }

        public int Add(ActiveDirectorySchemaClass schemaClass)
        {
            if (schemaClass == null)
            {
                throw new ArgumentNullException("schemaClass");
            }

            if (!schemaClass.isBound)
            {
                throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, schemaClass.Name));
            }

            if (!Contains(schemaClass))
            {
                return List.Add(schemaClass);
            }
            else
            {
                throw new ArgumentException(Res.GetString(Res.AlreadyExistingInCollection, schemaClass), "schemaClass");
            }
        }

        public void AddRange(ActiveDirectorySchemaClass[] schemaClasses)
        {
            if (schemaClasses == null)
            {
                throw new ArgumentNullException("schemaClasses");
            }

            foreach (ActiveDirectorySchemaClass schemaClass in schemaClasses)
            {
                if (schemaClass == null)
                {
                    throw new ArgumentException("schemaClasses");
                }
            }

            for (int i = 0; ((i) < (schemaClasses.Length)); i = ((i) + (1)))
            {
                this.Add((ActiveDirectorySchemaClass)schemaClasses[i]);
            }
        }

        public void AddRange(ActiveDirectorySchemaClassCollection schemaClasses)
        {
            if (schemaClasses == null)
            {
                throw new ArgumentNullException("schemaClasses");
            }

            foreach (ActiveDirectorySchemaClass schemaClass in schemaClasses)
            {
                if (schemaClass == null)
                {
                    throw new ArgumentException("schemaClasses");
                }
            }

            int currentCount = schemaClasses.Count;
            for (int i = 0; i < currentCount; i++)
            {
                this.Add(schemaClasses[i]);
            }
        }

        public void AddRange(ReadOnlyActiveDirectorySchemaClassCollection schemaClasses)
        {
            if (schemaClasses == null)
            {
                throw new ArgumentNullException("schemaClasses");
            }

            foreach (ActiveDirectorySchemaClass schemaClass in schemaClasses)
            {
                if (schemaClass == null)
                {
                    throw new ArgumentException("schemaClasses");
                }
            }

            int currentCount = schemaClasses.Count;
            for (int i = 0; i < currentCount; i++)
            {
                this.Add(schemaClasses[i]);
            }
        }

        public void Remove(ActiveDirectorySchemaClass schemaClass)
        {
            if (schemaClass == null)
            {
                throw new ArgumentNullException("schemaClass");
            }

            if (!schemaClass.isBound)
            {
                throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, schemaClass.Name));
            }

            for (int i = 0; i < InnerList.Count; i++)
            {
                ActiveDirectorySchemaClass tmp = (ActiveDirectorySchemaClass)InnerList[i];
                if (Utils.Compare(tmp.Name, schemaClass.Name) == 0)
                {
                    List.Remove(tmp);
                    return;
                }
            }
            throw new ArgumentException(Res.GetString(Res.NotFoundInCollection, schemaClass), "schemaClass");
        }

        public void Insert(int index, ActiveDirectorySchemaClass schemaClass)
        {
            if (schemaClass == null)
            {
                throw new ArgumentNullException("schemaClass");
            }

            if (!schemaClass.isBound)
            {
                throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, schemaClass.Name));
            }

            if (!Contains(schemaClass))
            {
                List.Insert(index, schemaClass);
            }
            else
            {
                throw new ArgumentException(Res.GetString(Res.AlreadyExistingInCollection, schemaClass), "schemaClass");
            }
        }

        public bool Contains(ActiveDirectorySchemaClass schemaClass)
        {
            if (schemaClass == null)
            {
                throw new ArgumentNullException("schemaClass");
            }

            if (!schemaClass.isBound)
            {
                throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, schemaClass.Name));
            }

            for (int i = 0; i < InnerList.Count; i++)
            {
                ActiveDirectorySchemaClass tmp = (ActiveDirectorySchemaClass)InnerList[i];
                if (Utils.Compare(tmp.Name, schemaClass.Name) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(ActiveDirectorySchemaClass[] schemaClasses, int index)
        {
            List.CopyTo(schemaClasses, index);
        }

        public int IndexOf(ActiveDirectorySchemaClass schemaClass)
        {
            if (schemaClass == null)
                throw new ArgumentNullException("schemaClass");

            if (!schemaClass.isBound)
            {
                throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, schemaClass.Name));
            }

            for (int i = 0; i < InnerList.Count; i++)
            {
                ActiveDirectorySchemaClass tmp = (ActiveDirectorySchemaClass)InnerList[i];
                if (Utils.Compare(tmp.Name, schemaClass.Name) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        protected override void OnClearComplete()
        {
            if (_isBound)
            {
                if (_classEntry == null)
                {
                    _classEntry = _schemaClass.GetSchemaClassDirectoryEntry();
                }

                try
                {
                    if (_classEntry.Properties.Contains(_propertyName))
                    {
                        _classEntry.Properties[_propertyName].Clear();
                    }
                }
                catch (COMException e)
                {
                    throw ExceptionHelper.GetExceptionFromCOMException(_context, e);
                }
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (_isBound)
            {
                if (_classEntry == null)
                {
                    _classEntry = _schemaClass.GetSchemaClassDirectoryEntry();
                }

                try
                {
                    _classEntry.Properties[_propertyName].Add(((ActiveDirectorySchemaClass)value).Name);
                }
                catch (COMException e)
                {
                    throw ExceptionHelper.GetExceptionFromCOMException(_context, e);
                }
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (_isBound)
            {
                if (_classEntry == null)
                {
                    _classEntry = _schemaClass.GetSchemaClassDirectoryEntry();
                }

                // because this collection can contain values from the superior classes,
                // these values would not exist in the classEntry 
                // and therefore cannot be removed
                // we need to throw an exception here
                string valueName = ((ActiveDirectorySchemaClass)value).Name;

                try
                {
                    if (_classEntry.Properties[_propertyName].Contains(valueName))
                    {
                        _classEntry.Properties[_propertyName].Remove(valueName);
                    }
                    else
                    {
                        throw new ActiveDirectoryOperationException(Res.GetString(Res.ValueCannotBeModified));
                    }
                }
                catch (COMException e)
                {
                    throw ExceptionHelper.GetExceptionFromCOMException(_context, e);
                }
            }
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (_isBound)
            {
                // remove the old value
                OnRemoveComplete(index, oldValue);
                // add the new value
                OnInsertComplete(index, newValue);
            }
        }

        protected override void OnValidate(Object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            if (!(value is ActiveDirectorySchemaClass))
                throw new ArgumentException("value");

            if (!((ActiveDirectorySchemaClass)value).isBound)
                throw new InvalidOperationException(Res.GetString(Res.SchemaObjectNotCommitted, ((ActiveDirectorySchemaClass)value).Name));
        }

        internal string[] GetMultiValuedProperty()
        {
            string[] values = new string[InnerList.Count];
            for (int i = 0; i < InnerList.Count; i++)
            {
                values[i] = ((ActiveDirectorySchemaClass)InnerList[i]).Name;
            }
            return values;
        }
    }
}
