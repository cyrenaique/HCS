using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using LibPlateAnalysis;

namespace HCSPlugin
{
	public class PluginDescriptor
	{
		private string name;
		private string menu;
		private Type type;
		private Assembly assem;
		private string fileName;
		private string author;

        public cScreening CurrentScreening;

		private bool wasVisible = false;

		/// <summary>
		/// The name of the plugin
		/// </summary>
		public string Name { get { return name; } }

		/// <summary>
		/// The menu path of the plugin
		/// </summary>
		public string MenuPath { get { return menu; } }

		/// <summary>
		/// The type the plugin
		/// </summary>
		public Type Type { get { return type; } }

		public string Author
		{
			get { return author; }
		}

		/// <summary>
		/// The full plugin file name(****.dll)
		/// </summary>
		public string FileName
		{
			get { return fileName; }
		}

		/// <summary>
		/// For Unique Plugin. This Assembly can be transfered to MD5 + SHA1 format
		/// </summary>
		public Assembly Assem { get { return assem; } }

		/// <summary>
		/// Create an object description of a plugin from which it is possible to instanciate it
		/// </summary>
		/// <param name="assem"></param>
		/// <param name="type"></param>
		/// <param name="menu"></param>
		/// <param name="name"></param>
		PluginDescriptor(Assembly assem, Type type, string menu,
			string name, string fileName, string author)
		{
			this.assem = assem;
			this.type = type;
			this.menu = menu;
			this.name = name;
			this.fileName = fileName;
			this.author = author;
		}


		/// <summary>
		/// Create an instance of the described plugin
		/// </summary>
		/// <returns>A plugin instance</returns>
		public Plugin Instanciate()
		{
			return (Plugin)Activator.CreateInstance(assem.GetType(type.ToString()));
		}

		/// <summary>
		/// Create the list of PluginAccessor 
		/// </summary>
		/// <param name="pluginDirectory">The plugin directory path</param>
		public static List<PluginDescriptor> GetList(string pluginDirectory)
		{
			List<PluginDescriptor> pluginList = new List<PluginDescriptor>();
			string[] test = Directory.GetFiles(pluginDirectory);
			foreach (string fileName in Directory.GetFiles(pluginDirectory, "*.dll"))
			{
				try
				{
					Assembly pluginAssembly = Assembly.LoadFrom(fileName);

					bool containsAPlugin = false;
					foreach (Type pluginType in pluginAssembly.GetTypes())
					{
						if (pluginType.IsPublic && !pluginType.IsAbstract)
						{

							if (pluginType.IsSubclassOf(typeof(Plugin)))
							{
								Hashtable ressources = GetRessources(pluginAssembly);
								string menu = (string)ressources["menu"];
								string name = (string)ressources["name"];
								string author = (string)ressources["author"];

								string tempfileName = (pluginAssembly.ToString());
								int offset = tempfileName.IndexOf(',');
								string pluginfileName = tempfileName.Substring(0, offset) + ".dll";

								if (menu == null || name == null || menu == "" && name == "")
									throw new PluginException("\nThis file is not an Plugin. It does not contain ressources keys and/or \"menu\" and/or \"name\" and/or \"author\".\n\n go to project/properties/ressources of " + fileName + " and add those keys.");
								pluginList.Add(new PluginDescriptor(pluginAssembly, pluginType, menu, name, pluginfileName, author));
								containsAPlugin = true;
								break;
							}

						}
					}
					if (!containsAPlugin)
						throw new PluginException("\nThis file is not an HCSAnalyzer Plugin.");

				}
				catch (FileLoadException e3)
				{
					MessageBox.Show(fileName + " cannot be read", "FileLoadException", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					//Console.WriteLine(@"FileLoadException: " + fileName + " cannot be read");
				}
				catch (ReflectionTypeLoadException e2)
				{
					MessageBox.Show(fileName + " is not an HCSAnalyzer Plugin DLL", "ReflectionTypeLoadException", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				catch (PluginException e1)
				{
					MessageBox.Show(fileName + " error:\n" + e1.Message, "Invalid Plugin", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				}
			}
			return pluginList;
		}


		// Retrieve key=>values from DLL ressource file
		private static Hashtable GetRessources(Assembly pluginAssembly)
		{
			string[] names = pluginAssembly.GetManifestResourceNames();
			Hashtable ressources = new Hashtable();
			foreach (string name in names)
			{
				if (name.EndsWith(".resources"))
				{
					ResourceReader reader = null;

					reader = new ResourceReader(pluginAssembly.GetManifestResourceStream(name));
					IDictionaryEnumerator en = reader.GetEnumerator();

					while (en.MoveNext())
					{
						// add key and value for plugin
						ressources.Add(en.Key, en.Value);
						//Console.WriteLine(en.Key + " => " + en.Value);
					}
					reader.Close();
				}
			}
			return ressources;
		}
	}

	public class PluginException : Exception
	{
		public PluginException()
			: base()
		{ }

		public PluginException(string message)
			: base(message)
		{ }
	}
}
