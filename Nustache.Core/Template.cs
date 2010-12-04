﻿using System.IO;
using Nustache.Core.Conventions;
using Nustache.Core.ValueProviders;

namespace Nustache.Core
{
    public class Template : Section
    {
        public Template()
            : base("#template") // I'm not happy about this fake name.
        {
        }

        /// <summary>
        /// Loads the template.
        /// </summary>
        /// <param name="reader">The object to read the template from.</param>
        /// <remarks>
        /// The <paramref name="reader" /> is read until it ends, but is not
        /// closed or disposed.
        /// </remarks>
        /// <exception cref="NustacheException">
        /// Thrown when the template contains a syntax error.
        /// </exception>
        public void Load(TextReader reader)
        {
            string template = reader.ReadToEnd();

            var scanner = new Scanner();
            var parser = new Parser();

            parser.Parse(this, scanner.Scan(template));
        }

        /// <summary>
        /// Renders the template.
        /// </summary>
        /// <param name="data">The data to use to render the template.</param>
        /// <param name="writer">The object to write the output to.</param>
        /// <param name="templateLocator">The delegate to use to locate templates for inclusion.</param>
        /// <remarks>
        /// The <paramref name="writer" /> is flushed, but not closed or disposed.
        /// </remarks>
        public void Render(object data, TextWriter writer, TemplateLocator templateLocator)
        {
            Render(data, writer, templateLocator, NamingConventionFactory.Default);
        }

        public void Render(object data, TextWriter writer, TemplateLocator templateLocator,
            NamingConvention namingConvention)
        {
            Render(data, writer, templateLocator,
                NamingConventionFactory.Create(namingConvention));
        }

        public void Render(object data, TextWriter writer, TemplateLocator templateLocator,
            INamingConvention namingConvention)
        {
            Render(data, writer, templateLocator,
                new ValueProviderCollection(namingConvention));
        }

        public void Render(object data, TextWriter writer, TemplateLocator templateLocator,
            IValueProviderCollection valueProviders)
        {
            var context = new RenderContext(this, data, writer, templateLocator, valueProviders);

            Render(context);

            writer.Flush();
        }
    }
}