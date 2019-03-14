// Types used in testing JSON serialization. 

namespace JsonApiTests
{
    public class Person {
        public long Id { get; set; }            // choose as identifier
        public string Name { get; set; }        // map to "firstname" - manual
        public string HideMe { get; set; }      // suppress - manual
        public string Field;                    // suppress if default - manual
        private string Private { get; set; }    // hide - not public
        private string PrivateField;            // hide - not public
        public Address Address { get; set; }            // show as embedded structure
        public Person Manager { get; set; }     // show remote id (and type?) only
    }

    public struct Address {
        public string Street { private get; set; }  // hide - no public getter
        public string City { get; set; }        // map to "city"
        public State State { get; set; }        // map to "state", display enum values
        public int? ZipCode;                    // suppress if default value - nullable
        public string Country { get; set; }    // suppress if default value - nullable 
    }

    public enum State { XX, AL, AR, AK, MI }
}