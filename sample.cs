<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>API Dashboard</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet">
    <style>
        /* Move accordion arrow before the text */
        .accordion-button::after {
            position: absolute;
            left: 1rem;
            margin-left: 0;
        }

        /* Create padding on left to accommodate absolutely positioned arrow */
        .accordion-button {
            padding-left: 3rem;
        }
    </style>
</head>

<body>
    <div class="container">
        <table class="table table-striped table-hover table-bordered border-primary border-2">
            <thead class="thead-light">
                <tr>
                    <th class="d-flex justify-content-between align-items-center">
                        <span>Member Inquiry API</span>
                        <div>
                            <button class="btn btn-primary btn-sm me-2">Execute</button>
                            <button class="btn btn-secondary btn-sm">Cancel</button>
                        </div>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" id="myTab" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="config-tab" data-bs-toggle="tab"
                                    data-bs-target="#config-pane" type="button" role="tab" aria-controls="config-pane"
                                    aria-selected="true">Configuration</button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="summary-tab" data-bs-toggle="tab"
                                    data-bs-target="#summary-pane" type="button" role="tab" aria-controls="summary-pane"
                                    aria-selected="false">Summary</button>
                            </li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content pt-3" id="myTabContent">
                            <div class="tab-pane fade show active" id="config-pane" role="tabpanel"
                                aria-labelledby="config-tab" tabindex="0">
                                <table class="table table-sm table-bordered mb-0">
                                    <thead class="table-light">
                                        <tr>
                                            <th>Setting</th>
                                            <th>Value</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Endpoint Method</td>
                                            <td>GET</td>
                                        </tr>
                                        <tr>
                                            <td>Timeout</td>
                                            <td>30 seconds</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="tab-pane fade" id="summary-pane" role="tabpanel" aria-labelledby="summary-tab"
                                tabindex="0">
                                <div class="accordion" id="summaryAccordion">
                                    <div class="accordion-item">
                                        <h2 class="accordion-header" id="headingOne">
                                            <button class="accordion-button align-items-center w-100" type="button"
                                                data-bs-toggle="collapse" data-bs-target="#collapseOne"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="fw-medium">Test Case: 1</span>
                                                <div class="ms-auto d-flex align-items-center gap-2">
                                                    <span class="text-muted small"><i class="bi bi-clock"></i>
                                                        120ms</span>
                                                    <span class="badge bg-success">200 OK</span>
                                                </div>
                                            </button>
                                        </h2>
                                        <div id="collapseOne" class="accordion-collapse collapse show"
                                            aria-labelledby="headingOne" data-bs-parent="#summaryAccordion">
                                            <div class="accordion-body">
                                                <ul class="list-group list-group-flush">
                                                    <li class="list-group-item">
                                                        <div class="fw-bold">Path Parameters</div>
                                                        <code>memberId:100293</code>
                                                    </li>
                                                    <li class="list-group-item">
                                                        <div class="fw-bold">Query Parameters</div>
                                                        <code>pageSize:1 | pageNumber:1000</code>
                                                    </li>
                                                    <li class="list-group-item">
                                                        <div class="fw-bold">Body Parameters</div>
                                                        <code>{ "memberId": "100293", "includeHistory": false }</code>
                                                    </li>

                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>


    <!-- Bootstrap Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>

</html>
