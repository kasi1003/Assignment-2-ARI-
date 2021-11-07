pdfjsLib.GlobalWorkerOptions.workerSrc = '../lib/pdf.js/pdf.worker.js';

var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 0.8,
    canvas = document.getElementById('documentViewer'),
    ctx = canvas.getContext('2d');

/**
 * Get page info from document, resize canvas accordingly, and render page.
 * @param num Page number.
 */
function renderPage(num) {
    pageRendering = true;
    // Using promise to fetch the page
    pdfDoc.getPage(num).then(function (page) {
        console.log('Page loaded');
        var viewport = page.getViewport({ scale: scale });
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRendering = false;
            if (pageNumPending !== null) {
                // New page rendering is pending
                renderPage(pageNumPending);
                pageNumPending = null;
            }
            console.log('Page rendered');
        });
    });

    // Update page counters
    document.getElementById('page_num').textContent = num;
}

/**
 * If another page rendering in progress, waits until the rendering is
 * finised. Otherwise, executes rendering immediately.
 */
function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}

/**
 * Displays previous page.
 */
function onPrevPage() {
    if (pageNum <= 1) {
        return;
    }
    pageNum--;
    queueRenderPage(pageNum);
}
document.getElementById('prev').addEventListener('click', onPrevPage);

/**
 * Displays next page.
 */
function onNextPage() {
    if (pageNum >= pdfDoc.numPages) {
        return;
    }
    pageNum++;
    queueRenderPage(pageNum);
}
document.getElementById('next').addEventListener('click', onNextPage);

/**
 * Asynchronously downloads PDF.
 */
function loadDocumentViewer(content) {
    var pdfData = atob(content);
    var loadingTask = pdfjsLib.getDocument({ data: pdfData });
    loadingTask.promise.then(function (pdfDoc_) {
        pdfDoc = pdfDoc_;
        console.log('PDF loaded');
        document.getElementById('page_count').textContent = pdfDoc.numPages;
        // Initial/first page rendering
        renderPage(pageNum);
    }, function (reason) {
        // PDF loading error
        console.error(reason);
    });
};

function ViewDocument(docId) {
    $.ajax({
        url: "/Document/GetDocument?docId=" + docId,
        type: "GET",
        traditional: true,
        traditional: true,
        success: function (content) {
            loadDocumentViewer(content);
            $('#pdfViewerModal').modal();
            $('#pdfViewerModal').modal('open');
        },
        error: function () {
        }
    });
};
