using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterPointCoreSecurity.Model
{
    public enum EnumMessageId
    {
        /// <summary>
        /// LS101	Any kind of operation that is called is done successful
        /// LS200	Run successfully from front end but doesnot make any hit to DB that is for INSERT UPDATE DELETE , 0 records are update.
        /// LS201	Comparison failed among records of tables
        /// LS202	Comparison failed between records and external values
        /// LS203	Business logic failed according to calculation        
        /// LS204   Data is not available for any particular operation,like delete,save cannot be performed because data is not found.
        /// LS205	Business logic failed according to Bsuniess Validation
        /// LS250	For bulk data some operations are successfull and some are failed.
        /// LS251	Exception Occured
        /// </summary>      

        /// <summary>
        /// LS101	Any kind of operation that is called is done successful        
        /// </summary>      
        LS101,
        /// <summary>
        /// LS200	Run successfully from front end but doesnot make any hit to DB that is for INSERT UPDATE DELETE , 0 records are update.
        /// </summary>      
        LS200,
        /// <summary>
        /// LS201	Comparison failed among records of tables
        /// </summary>      
        LS201,
        /// <summary>
        /// LS202	Comparison failed between records and external values
        /// </summary>      
        LS202,
        /// <summary>
        /// LS203	Business logic failed according to calculation        
        /// </summary>      
        LS203,
        /// <summary>
        /// LS204   Data is not available for any particular operation,like delete,save cannot be performed because data is not found.
        /// </summary>      
        LS204,
        /// <summary>
        /// LS205	Business logic failed according to Bsuniess Validation
        /// </summary>      
        LS205,
        /// <summary>
        /// LS250	For bulk data some operations are successfull and some are failed.
        /// </summary>      
        LS250,
        /// <summary>
        /// LS251	Exception Occured
        /// </summary>      
        LS251
    }

    public enum EnumActionType
    {
        INSERT, UPDATE, DELETE
    }
}
