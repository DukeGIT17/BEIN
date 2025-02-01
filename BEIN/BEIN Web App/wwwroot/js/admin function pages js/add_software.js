const photoUpload = document.getElementById('photo');
const visiblePhotoBtn = document.getElementById('visible-photo-upload-btn');
const alertBox = document.getElementById('upload-alert');
const linkElement = document.createElement("link");

linkElement.setAttribute("rel", "stylesheet");
linkElement.setAttribute("href", "/css/admin function pages css/add_software.css");

document.head.appendChild(linkElement);

function upload() {
    photoUpload.click();
}

function bulkUpload() {
    document.getElementById('bulk-upload').click();
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
    featureName.setAttribute('name', 'feature_name');

    const featureDescInput = document.createElement('input');
    featureDescInput.setAttribute('type', 'text');
    featureDescInput.setAttribute('value', featureDesc.value);
    featureDescInput.setAttribute('readonly', true);
    featureDescInput.setAttribute('name', 'feature_desc');

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

