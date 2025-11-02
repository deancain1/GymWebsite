let html5QrCode = null; // Global instance so we can stop it later

window.qrScanner = {
    init: (dotNetHelper, scannerId = "qr-reader") => {
        if (!window.Html5Qrcode) {
            console.error("html5-qrcode not loaded");
            return;
        }

        const scannerElement = document.getElementById(scannerId);
        if (!scannerElement) {
            console.error(`Element with id '${scannerId}' not found.`);
            return;
        }

        // Make container ready for overlay
        scannerElement.style.position = "relative";
        scannerElement.style.overflow = "hidden";

        // === Simple center frame ===
        const frame = document.createElement("div");
        frame.style.position = "absolute";
        frame.style.top = "50%";
        frame.style.left = "50%";
        frame.style.width = "250px";
        frame.style.height = "250px";
        frame.style.transform = "translate(-50%, -50%)";
        frame.style.border = "3px solid #00ff00"; // plain green border
        frame.style.pointerEvents = "none";
        frame.style.zIndex = "10";

        scannerElement.appendChild(frame);

        // === Initialize QR Scanner ===
        html5QrCode = new Html5Qrcode(scannerId);

        let isCooldown = false;

        const startScanner = () => {
            Html5Qrcode.getCameras()
                .then(devices => {
                    if (devices && devices.length) {
                        // Prefer back camera
                        const cameraId =
                            devices.find(d => d.label.toLowerCase().includes("back"))?.id ||
                            devices[0].id;

                        html5QrCode.start(
                            cameraId,
                            {
                                fps: 10,
                                qrbox: { width: 250, height: 250 }, // same as frame
                            },
                            async decodedText => {
                                if (isCooldown) return;
                                isCooldown = true;

                                console.log("Scanned QR:", decodedText);

                                try {
                                    await dotNetHelper.invokeMethodAsync("OnQrScanned", decodedText);
                                } catch (err) {
                                    console.error("Error invoking Blazor method:", err);
                                }

                                // 3s cooldown to prevent duplicates
                                setTimeout(() => (isCooldown = false), 3000);
                            },
                            errorMessage => {
                                // Optional: handle errors quietly
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
