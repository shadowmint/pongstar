using System;
using System.Collections.Generic;
using n.Core;

namespace n.Platform
{
  /** Callback for completion and status */
  public delegate void nDbRecordCb<T> (T value);

	/** Platform bindings for basic async persistence operations */
	public interface nDb
	{
    /** Setup the database to accept records of a particular */
    void Setup<T>(nDbRecordCb<bool> cb) where T : nDbRecord;
    
    /** Save a new db record */
    void Insert<T>(T record, nDbRecordCb<bool> cb) where T : nDbRecord;
    
    /** Update an existing db record */
    void Update<T>(T record, nDbRecordCb<bool> cb) where T : nDbRecord;
    
    /** Delete an existing db record */
    void Delete<T>(T record, nDbRecordCb<bool> cb) where T : nDbRecord;
    
    /** Return a count of records */
    void Count<T>(nDbRecordCb<int> cb) where T : nDbRecord;
    
    /** Get a specific db record */
    void Get<T>(long id, nDbRecordCb<T> cb) where T : nDbRecord;
    
    /** Get a specific db record */
    void All<T>(long offset, long limit, nDbRecordCb<IEnumerable<T>> cb) where T : nDbRecord;
    
    /** Delete all records of a given type */
    void Clear<T>(nDbRecordCb<bool> cb) where T : nDbRecord;
	}
}

