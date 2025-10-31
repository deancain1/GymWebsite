let html5QrCode = null; // Global instance so we can stop it later

window.qrScanner = {
    init: (dotNetHelper, scannerId = "qr-reader") => {
        if (!window.Html5Qrcode) {
            console.error("html5-qrcode not loaded");
            return;
        }

        html5QrCode = new Html5Qrcode(scannerId);

        let isCooldown = false;

        const startScanner = () => {
            Html5Qrcode.getCameras()
                .then(devices => {
                    if (devices && devices.length) {
                        // Prefer back camera if available
                        const cameraId =
                            devices.find(d => d.label.toLowerCase().includes("back"))?.id ||
                            devices[0].id;

                        html5QrCode.start(
                            cameraId,
                            { fps: 10, qrbox: { width: 500, height: 500 } },
                            async decodedText => {
                                if (isCooldown) return;
                                isCooldown = true;

                                console.log("Scanned QR:", decodedText);

                                try {
                                    await dotNetHelper.invokeMethodAsync("OnQrScanned", decodedText);
                                } catch (err) {
                                    console.error("Error invoking Blazor method:", err);
                                }

                                // Cooldown for 3 seconds to prevent duplicate scans
                                setTimeout(() => isCooldown = false, 3000);
                            },
                            errorMessage => {
                                // Optional: handle scan errors
                                // console.warn(`QR scan error: ${errorMessage}`);
                            }
                        ).catch(err => {
                            console.error("Failed to start camera:", err);
                            alert("Unable to start camera. Check permissions or try another device.");
                        });
                    } else {
                        alert("No camera devices found.");
                    }
                })
                .catch(err => {
                    console.error("Error getting cameras:", err);
                    alert("Error accessing cameras.");
                });
        };

        startScanner();
    },

    stop: () => {
        if (html5QrCode) {
            html5QrCode.stop()
                .then(() => {
                    html5QrCode.clear();
                    console.log("QR Scanner stopped and cleared.");
                })
                .catch(err => {
                    console.error("Failed to stop QR scanner:", err);
                });
        }
    }
};
