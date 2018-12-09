﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Sds.Osdr.BddTests.Traits
{
	public class ProcessingTraitDiscoverer : ITraitDiscoverer
	{
		public const string Category = "Processing";

		public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
		{
			var args = (List<Object>)traitAttribute.GetConstructorArguments();
			var groups = (Array)args[0];

			foreach (var nameGroup in groups)
			{
				yield return new KeyValuePair<string, string>(Category, nameGroup.ToString());
			}
		}
	}
}
