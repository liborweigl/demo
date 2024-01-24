import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ReportProducerService } from 'src/app/core/services/report-producer.service';
import { IDocumentDraft } from 'src/app/shared/AI/IDocumentDraft';
import { IDocumentDraftSection } from 'src/app/shared/AI/IDocumentDraftSection';

@Component({
  selector: 'draft-document',
  templateUrl: './draft-document.component.html',
  styleUrls: ['./draft-document.component.scss']
})
export class DraftDocumentComponent implements OnChanges {

  @Input() public draftDocument: IDocumentDraft;

  draftDocumentFg: FormGroup;
  showSectionDraftDocumentLoader: boolean = false;

  constructor(private fb: FormBuilder, private reportProducerService: ReportProducerService) {

    this.draftDocumentFg = this.fb.group({
      ContentTextId: [0],
      DocumentDraftSections: this.fb.array([
        this.fb.group({
          DocumentDraftSectionId: [0, []],
          DocumentHeadingId: [0, []],
          DraftHeading: ['', []],
          WordLenghtCount: [0, []],
          Context: ['', []],
          Text: ['', []],
        }),
      ]),
    });
  }

  ngOnChanges(): void {

    for (var i = 1; i < this.draftDocument.DocumentDraftSections.length; i++) {
      this.addRowforDocumentHeading();
    }

    this.draftDocumentFg.patchValue(this.draftDocument);
  }

  addRowforDocumentHeading() {

    this.DraftDocumentSections.push(
      this.fb.group({
        DocumentDraftSectionId: [0, []],
        DocumentHeadingId: [0, []],
        DraftHeading: ['', []],
        WordLenghtCount: [0, []],
        Context: ['', []],
        Text: ['', []],
      })
    );
  }

  get DraftDocumentSections() {

    return this.draftDocumentFg.get('DocumentDraftSections') as FormArray;

  }

  getHeadingText(index: number) {
    var draftHeading = this.DraftDocumentSections.controls[index].get('DraftHeading').value;

    if (draftHeading == null || draftHeading == undefined || draftHeading == '') {
      draftHeading = (index + 1).toString();
    }
    else {
      draftHeading = draftHeading.trim();
    }

    return draftHeading;
  }

  getDraftText(index: number) {
    var draftHeading = this.DraftDocumentSections.controls[index].get('Text').value;

    if (draftHeading == null || draftHeading == undefined || draftHeading == '') {
      draftHeading = "";
    }
    else {
      draftHeading = draftHeading.trim();
    }

    return draftHeading;
  }

  generateContentForSection(index: number): void {

    var draftDocumentTemp = this.draftDocumentFg.value as IDocumentDraft;
    this.draftDocument.DocumentDraftSections = draftDocumentTemp.DocumentDraftSections;
    this.draftDocument.SectionIndex = index;
    this.showSectionDraftDocumentLoader = true;
    this.reportProducerService.generateSectionDraftDocument(this.draftDocument)
      .subscribe((draftSectionDocument: IDocumentDraftSection) => {
        this.draftDocument.DocumentDraftSections[index] = draftSectionDocument;

        this.draftDocumentFg.patchValue(this.draftDocument);
        this.showSectionDraftDocumentLoader = false;
      });

  }

  downloadWordDocumentWithResponse(): void { } //todo
  copyContentToClipBoard(): void { } //todo


}
