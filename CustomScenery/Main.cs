using UnityEngine;

namespace Custom_Scenery
{
    public class Main : IMod
    {
        private GameObject _go;
        string name;
        string description;
        public void onEnabled()
        {
            _go = new GameObject();

            _go.AddComponent<Loader>();

            _go.GetComponent<Loader>().Path = Path;

            _go.GetComponent<Loader>().Identifier = Identifier;

            _go.GetComponent<Loader>().LoadScenery();

            name = _go.GetComponent<Loader>().modName;

            description = _go.GetComponent<Loader>().modDiscription;
        }

        public void onDisabled()
        {
            _go.GetComponent<Loader>().UnloadScenery();

            Object.Destroy(_go);
        }

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public string Path { get; set; }
        public string Identifier { get; set; }
    }
}
