const photoUpload = document.getElementById('photo');
const visiblePhotoBtn = document.getElementById('visible-photo-upload-btn');
const alertBox = document.getElementById('upload-alert');
const bulkUploadBtn = document.getElementById('bulk-upload');
const dropArea = document.getElementById('upload-field');
const displayArea = document.getElementById('uploaded-file');
const detailsArea = document.getElementById('file-details');
const submitBtn = document.getElementById('submit');
const linkElement = document.createElement("link");
let file;

linkElement.setAttribute("rel", "stylesheet");
linkElement.setAttribute("href", "/css/admin function pages css/add_software.css");
document.head.appendChild(linkElement);

function upload() {
    photoUpload.click();
}

function bulkUpload() {
    bulkUploadBtn.click();
}

function removeFile(inputId) {
    const uploadField = document.getElementById(inputId);

    if (uploadField) {
        uploadField.value = '';
        visiblePhotoBtn.style.backgroundColor = 'transparent';
        visiblePhotoBtn.setAttribute('onclick', 'upload()')
        alertBox.innerText = '';
        alertBox.style.display = 'none';

        setTimeout(function () {
            visiblePhotoBtn.addEventListener('change', function (event) {
                change(event);
            });
        }, 1000);
    }
}

photoUpload.addEventListener('change', function (event) {
    change(event);
});

function change(event) {
    const file = event.target.files[0];

    if (file) {
        visiblePhotoBtn.style.backgroundColor = 'LightGreen';
        visiblePhotoBtn.setAttribute('onclick', 'removeFile(\'upload-alert\')')
        alertBox.innerText = file.name + ' uploaded successfully';
        alertBox.style.display = 'block';
        setTimeout(function () {
            alertBox.style.display = 'none';
        }, 1500)
    }
}

let removeFeatureBtns = document.querySelectorAll('.remove-feature-btn');

removeFeatureBtns.forEach(btn => {
    btn.addEventListener('click', function () {
        const parent = this.parentElement;
        if (parent) {
            parent.remove();
        }
    });
});

function addFeature() {
    const feature = document.getElementById('feature');
    const featureDesc = document.getElementById('feature-desc');

    const newFeature = document.createElement('div');
    newFeature.classList.add('feature');

    const featureData = document.createElement('div');
    featureData.classList.add('feature-data');

    const removeFeatureBtn = document.createElement('button');
    removeFeatureBtn.classList.add('remove-feature-btn');
    removeFeatureBtn.setAttribute('type', 'button');

    const xIcon = document.createElement('i');
    xIcon.classList.add('fa-solid', 'fa-xmark');

    const featureName = document.createElement('input');
    featureName.setAttribute('type', 'text');
    featureName.setAttribute('value', feature.value);
    featureName.setAttribute('readonly', true);

    const featureDescInput = document.createElement('input');
    featureDescInput.setAttribute('type', 'text');
    featureDescInput.setAttribute('value', featureDesc.value);
    featureDescInput.setAttribute('readonly', true);

    document.getElementById('added-features').appendChild(newFeature);
    newFeature.appendChild(featureData);
    removeFeatureBtn.appendChild(xIcon);
    newFeature.appendChild(removeFeatureBtn);
    featureData.appendChild(featureName);
    featureData.appendChild(featureDescInput);

    removeFeatureBtn.addEventListener('click', function () {
        const parent = this.parentElement;
        if (parent) {
            parent.remove();
        }
    });
}

['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropArea.addEventListener(eventName, (e) => e.preventDefault());
});

bulkUploadBtn.addEventListener('change', function (event) {
    file = event.target.files[0];
    handleFile();
});

dropArea.addEventListener('dragover', () => dropArea.classList.add('drag-over'));
dropArea.addEventListener('dragleave', () => dropArea.classList.remove("drag-over"));

dropArea.addEventListener('drop', (event) => {
    dropArea.classList.remove('drag-over');
    file = event.dataTransfer.files[0];
    handleFile();
});

function handleFile() {
    if (!file) return;

    const allowedExtensions = ['xlsx', 'xls'];
    const fileExtension = file.name.split('.').pop().toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
        alert(`Invalid file type '${fileExtension}'. Please select an Excel file (.xlsx, .xls).`);
    } else {
        const fileTitle = detailsArea.querySelector("strong");
        const fileTypeSize = detailsArea.querySelector("span");

        if (fileTitle && fileTypeSize) {
            fileTitle.innerText = file.name;
            fileTypeSize.innerHTML = `<strong>.${fileExtension.toUpperCase()}</strong> | ${formatFileSize(file.size)}`;
            displayArea.style.display = 'flex';
        } else {
            alert('Something went wrong with the file upload! Please try again, if the issue persists, try refreshing the page.')
        }
    }
}

function formatFileSize(bytes) {
    const sizes = ["Bytes", "KB", "MB", "GB", "TB"];
    let i = 0;
    while (bytes >= 1024 && i < sizes.length - 1) {
        bytes /= 1024;
        i++;
    }
    return `${bytes.toFixed(2)} ${sizes[i]}`;
}

submitBtn.addEventListener('click', async (e) => {
    e.preventDefault()

    if (file) {
        const formData = new FormData();
        formData.append('file', file);

        let response = await fetch('https://localhost:7012/api.bein.com/AdminFunctions/BulkSoftwareUpload', {
            method: 'POST',
            body: formData
        })

        if (response.ok) {
            dropArea.style.backgroundColor = 'rgb(36, 236, 36, .5)';
            setTimeout(() => {
                dropArea.style.backgroundColor = 'var(--background-color)'
                fetch('https://localhost:7222/AdminFunctions/AddSoftware');
            }, 1500);
        } else {
            alert(`Upload failed! ${await response.text()}`);
        }
    } else {
        alert('Please select an excel file to upload.')
    }
});