export function ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle) {
    const canvas = document.getElementById("canvas");
    const ctx = canvas.getContext("2d");

    // Draw the ellipse
    ctx.beginPath();
    ctx.lineWidth = 2;
    ctx.ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle);
    ctx.stroke();

    ctx.lineWidth = 1;

    // 175, 100
    ctx.beginPath();
    ctx.moveTo(x, y);
    ctx.lineTo(y + radiusY, y);
    ctx.stroke();

    // 100, 150
    ctx.beginPath();
    ctx.setLineDash([5, 5]);
    ctx.moveTo(x, y);
    ctx.lineTo(x, y + radiusX);
    ctx.stroke();

    // 100, 50
    ctx.beginPath();
    ctx.setLineDash([0, 0]);
    ctx.moveTo(x, y);
    ctx.lineTo(x, y - radiusX);
    ctx.stroke();

    // 50, 100
    ctx.beginPath();
    ctx.setLineDash([5, 5]);
    ctx.moveTo(x, y);
    ctx.lineTo(x - radiusY, y);
    ctx.stroke();

    ctx.setLineDash([0, 0]);

    ctx.beginPath();
    ctx.arc(y + radiusY, x, (x * 0.05), 0, 2 * Math.PI);
    ctx.fillStyle = "blue";
    ctx.fill();
    ctx.stroke()

    ctx.beginPath();
    ctx.arc(x + (x / 3), y, (x * 0.1), 0, 2 * Math.PI);
    ctx.fillStyle = "orange";
    ctx.fill();
    ctx.stroke()
}