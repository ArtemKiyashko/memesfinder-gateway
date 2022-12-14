﻿using System;
using Newtonsoft.Json;

namespace MemesFinderGateway.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToJson(this object update) => update is null ? null : JsonConvert.SerializeObject(update);
	}
}

