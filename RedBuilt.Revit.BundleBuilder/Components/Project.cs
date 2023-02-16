using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Components
{
    public class Project
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string Location { get; private set; }
        public Queue<Wall> Walls { get; private set; }
        public Queue<Bundle> Bundles { get; private set; }
        
        public Project(string name, string id, string location, Queue<Wall> walls)
        {
            this.Name = name;
            this.Id = id;
            this.Location = location;
            this.Walls = walls;
        }

        public void Bundle()
        {
            throw new Exception("Not Implemented");
        }

    }
}
