const pdfInstances = new Map();

export function cleanupPdfViewer(containerId) {
    const instance = pdfInstances.get(containerId);
    if (instance) {
        try {
            if (instance.pdfViewer) instance.pdfViewer.setDocument(null);
            if (instance.eventBus?.destroy) instance.eventBus.destroy();
            if (instance.pdfDoc) instance.pdfDoc.destroy();
        } catch (e) { }
        pdfInstances.delete(containerId);
    }
    const container = document.getElementById(containerId);
    if (container) container.innerHTML = '';
}

export function renderPdf(containerId, base64Data) {
    return new Promise(async (resolve, reject) => {
        console.log("[JS] renderPdf: Rozpoczęto.");
        try {
            console.log('dupa');
            await cleanupPdfViewer(containerId);
            const mainContainer = document.getElementById(containerId);
            if (!mainContainer) {
                return reject(`PDF container not found: ${containerId}`);
            }

            mainContainer.innerHTML = '';
            mainContainer.style.position = 'absolute';
            mainContainer.style.width = '100%';
            mainContainer.style.height = '100%';
            const viewerDiv = document.createElement('div');
            viewerDiv.id = 'viewer';
            viewerDiv.className = 'pdfViewer';
            mainContainer.appendChild(viewerDiv);

            const pdfjsLib = await import('./pdfjs/pdf.js');
            const { PDFViewer, EventBus, PDFLinkService, GenericL10n } = await import('./pdfjs/web/pdf_viewer.js');
            
            pdfjsLib.GlobalWorkerOptions.workerSrc = './pdf.worker.js';

            const eventBus = new EventBus();
            const pdfDoc = await pdfjsLib.getDocument({ data: atob(base64Data) }).promise;

            eventBus.on('annotationlayerrendered', (event) => {
                if (event.pageNumber === pdfDoc.numPages) {
                    resolve();
                }
            });

            const l10n = new GenericL10n("en-US");
            const linkService = new PDFLinkService({ eventBus });
            const pdfViewer = new PDFViewer({ container: mainContainer, viewer: viewerDiv, eventBus, l10n, renderInteractiveForms: true, textLayerMode: 2, annotationMode: pdfjsLib.AnnotationMode.ENABLE_FORMS });

            linkService.setViewer(pdfViewer);
            pdfViewer.setDocument(pdfDoc);
            linkService.setDocument(pdfDoc, null);

            pdfInstances.set(containerId, { pdfDoc, pdfViewer, eventBus, linkService });
        } catch (error) {
            reject(error);
        }
    });
}

export async function getFormDataFromPdf(containerId) {
    const mainContainer = document.getElementById(containerId);
    if (!mainContainer) return null;
    try {
        const formData = {};
        const elements = mainContainer.querySelectorAll('input, textarea, select');
        elements.forEach(el => {
            if (el.name) {
                if (el.type === 'checkbox') formData[el.name] = el.checked;
                else if (el.type === 'radio') {
                    const checkedRadio = mainContainer.querySelector(`input[name="${el.name}"]:checked`);
                    formData[el.name] = checkedRadio ? checkedRadio.value : '';
                } else formData[el.name] = el.value ?? '';
            }
        });
        const json = JSON.stringify(formData);
        return json;
    } catch (error) {
        return null;
    }
}

export async function loadFormDataIntoPdf(containerId, jsonData) {
    const mainContainer = document.getElementById(containerId);
    const instance = pdfInstances.get(containerId);
    if (!mainContainer || !instance || !instance.pdfDoc) {
        return;
    }
    try {
        const formData = JSON.parse(jsonData);
        for (const fieldName in formData) {
            if (Object.prototype.hasOwnProperty.call(formData, fieldName)) {
                const elements = mainContainer.querySelectorAll(`input[name="${fieldName}"], textarea[name="${fieldName}"], select[name="${fieldName}"]`);
                if (elements.length > 0) {
                    const valueToSet = formData[fieldName];
                    const el = elements[0];
                    const type = el.type?.toLowerCase();

                    if (type === 'checkbox') el.checked = !!valueToSet;
                    else if (type === 'radio') {
                        const radioToSelect = mainContainer.querySelector(`input[name="${fieldName}"][value="${valueToSet}"]`);
                        if (radioToSelect) radioToSelect.checked = true;
                    } else el.value = valueToSet;

                    const fieldObjects = await instance.pdfDoc.getFieldObjects();
                    if (fieldObjects && fieldObjects[fieldName]) {
                        for (const field of fieldObjects[fieldName]) {
                            instance.pdfDoc.annotationStorage.setValue(field.id, { fieldValue: valueToSet });
                        }
                    }
                } 
            }
        }
    } catch (error) {
    }
}


export function isFormReady(containerId) {
    const mainContainer = document.getElementById(containerId);
    if (!mainContainer) return false;
    const element = mainContainer.querySelector('input, textarea, select');
    return element != null;
}