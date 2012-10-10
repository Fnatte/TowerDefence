using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence
{
	interface ITextRenderer
	{
		FactoryDWrite FactoryDWrite { get; }
	}
}
