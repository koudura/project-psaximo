namespace Fornax.Net.Tests.Tools
{
    public interface ITrie<T> 
    {
        bool Delete(T[] word);
        bool Delete(string word);
        bool Equals(object obj);
        int GetHashCode();
        void Insert(T[] word);
        void Insert(string word);
        bool Search(T[] word);
        bool Search(string word);
        
        string ToString();
    }
}