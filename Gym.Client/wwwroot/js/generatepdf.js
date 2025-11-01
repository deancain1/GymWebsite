window.generatePdf = async function (fullName, email, phone, status, appliedDate, qrBase64) {
    const { jsPDF } = window.jspdf;


    const doc = new jsPDF({
        orientation: "landscape",
        unit: "mm",
        format: "a6"
    });


    doc.setDrawColor(50, 50, 50);
    doc.setLineWidth(1);
    doc.roundedRect(5, 5, 138, 95, 5, 5);


    doc.setFontSize(18);
    doc.setFont("helvetica", "bold");
    doc.text("Membership Card", 74, 18, { align: "center" });


    doc.setLineWidth(0.5);
    doc.line(20, 25, 125, 25);


    doc.setFontSize(12);
    doc.setFont("helvetica", "normal");

    let y = 40;
    let xLeft = 20;
    let xRight = 60;

    doc.text("Full Name:", xLeft, y);
    doc.text(fullName, xRight, y);
    y += 10;

    doc.text("Email:", xLeft, y);
    doc.text(email, xRight, y);
    y += 10;

    doc.text("Phone:", xLeft, y);
    doc.text(phone, xRight, y);
    y += 10;

    doc.text("Status:", xLeft, y);
    doc.text(status, xRight, y);
    y += 10;

    doc.text("Applied:", xLeft, y);
    doc.text(appliedDate, xRight, y);


    if (qrBase64) {
        doc.addImage(qrBase64, "PNG", 100, 40, 35, 35);
    }


    doc.setFontSize(10);
    doc.setFont("helvetica", "italic");
    doc.text("Gym Membership Verification", 74, 90, { align: "center" });

  
    doc.save(fullName + "_MembershipCard.pdf");
}
