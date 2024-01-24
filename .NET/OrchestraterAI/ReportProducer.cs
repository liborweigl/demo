using Core.Dtos.DocumentTemplateAI;
using Microsoft.SemanticKernel;
using OrchestraterAI.Extentions;

namespace OrchestraterAI
{
    public class ReportProducer : IReportProducer
    {
        private readonly Kernel _kernel;

        public ReportProducer(IKernelBuilder kernelBuilder)
        {
            _kernel = kernelBuilder.Build();

            SemanticKernelExtentions.RegisterPluginsAsync(_kernel, "Plugins");
        }

        public async Task<DraftDocumentDto> ProduceReport(DraftDocumentDto draftDocument)
        {
            var documentTemplate = draftDocument.DocumentTemplate;

            foreach (var documentSection in draftDocument.DocumentDraftSections)
            {
                string? text = await GenerateTextForSection(draftDocument, documentTemplate, documentSection);

                documentSection.Text = text;
            }

            return draftDocument;
        }

 
        public async Task<DocumentDraftSectionDto> ProduceSelectedSectionReport(int sectionIndex, DraftDocumentDto draftDocument)
        {
            var documentTemplate = draftDocument.DocumentTemplate;

            var documentSection = draftDocument.DocumentDraftSections[sectionIndex];

            documentSection.Text = await GenerateTextForSection(draftDocument, documentTemplate, documentSection);

            return documentSection;
        }

        private async Task<string?> GenerateTextForSection(DraftDocumentDto draftDocument, DocumentTemplateDto documentTemplate, DocumentDraftSectionDto documentSection)
        {
            return await _kernel.InvokeAsync<string>(_kernel.Plugins["ReportProducerPlugin"]["ReportSectionProducer"], new()
            {
                ["Persona"] = documentTemplate.Persona,
                ["Title"] = documentTemplate.Title,
                ["Heading"] = draftDocument.DocumentDraftSections,
                ["Context"] = documentSection.Context,
                ["Text"] = documentSection.Text,
                ["MaxWordCount"] = documentSection.WordLengthCount
            });
        }


    }
}