using System.Collections.Generic;

namespace Model.Dao
{
    // Interface define o que uma classe deve ser capaz de fazer,
    // mas não como ela o faz (Material 06).
    public interface Intermediario<nomeClasse>
    {
        void create(nomeClasse obj);
        void delete(nomeClasse obj);
        void update(nomeClasse obj);
        bool find(nomeClasse obj);
        List<nomeClasse> findAll();
    }
}
