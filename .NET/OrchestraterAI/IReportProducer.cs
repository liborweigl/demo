using Core.Dtos.DocumentTemplateAI;

namespace OrchestraterAI
{
    public interface IReportProducer
    {
        Task<DraftDocumentDto> ProduceReport(DraftDocumentDto draftDocument);

        Task<DocumentDraftSectionDto> ProduceSelectedSectionReport(int sectionIndex, DraftDocumentDto draftDocument);
    }
}