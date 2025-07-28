export async function renderPdf(containerId, base64Data) {
    
    
    const mainContainer = document.getElementById(containerId);
    if (!mainContainer) {
        console.error("PDF container not found:", containerId);
        return;
    }
    
    mainContainer.innerHTML = '';
    mainContainer.style.position = 'absolute'; 
    mainContainer.style.width = '100%';
    mainContainer.style.height = '100%';

    const viewerDiv = document.createElement('div');
    viewerDiv.id = 'viewer';
    viewerDiv.className = 'pdfViewer';
    mainContainer.appendChild(viewerDiv);

    try {
        const pdfjsLib = await import('/js/pdfjs/pdf.mjs');
        window.pdfjsLib = pdfjsLib;
        const { PDFViewer, EventBus, PDFLinkService, GenericL10n } = await import('/js/pdfjs/web/pdf_viewer.mjs');
        pdfjsLib.GlobalWorkerOptions.workerSrc = '/js/pdfjs/pdf.worker.mjs';

        const l10n = new GenericL10n("en-US");
        const binaryString = atob(base64Data);
        const bytes = new Uint8Array(binaryString.length);
        for (let i = 0; i < binaryString.length; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }

        const eventBus = new EventBus();
        const linkService = new PDFLinkService({ eventBus });
        
        const pdfViewer = new PDFViewer({
            container: mainContainer,
            viewer: viewerDiv,
            eventBus: eventBus,
            linkService: linkService,
            l10n: l10n,
            renderInteractiveForms: true,
            textLayerMode: 2,
            annotationMode: pdfjsLib.AnnotationMode.ENABLE_FORMS,
        });

        linkService.setViewer(pdfViewer);
        
        const pdfDoc = await pdfjsLib.getDocument({
            data: bytes,
            enableXfa: true
        }).promise;

        pdfViewer.setDocument(pdfDoc);
        linkService.setDocument(pdfDoc, null);

    } catch (error) {
        console.error('Error loading PDF:', error);
        mainContainer.innerHTML = '<div style="color: red;">Error rendering PDF. Check console.</div>';
    }
}