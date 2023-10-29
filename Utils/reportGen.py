import argparse
import matplotlib.pyplot as plt
import csv
from fpdf import FPDF
from datetime import date, datetime

FONT = 'Arial'


class MyPDF(FPDF):
    def __init__(self):
        super().__init__()

    def footer(self):
        self.set_y(-15)
        self.set_font(FONT, '', 12)
        self.cell(0, 8, 'Page ' + str(self.page_no()), 0, 0, 'C')


def generate_pdf(max_height, max_speed, max_angle_of_attack, displacement, date, author, speedChart, heightChart):
    pdf = MyPDF()
    pdf.add_page()

    pdf.set_font(FONT, 'B', 26)
    pdf.cell(w=0, h=20, txt="Rocket Flight Report", ln=1)

    pdf.set_font(FONT, '', 14)
    pdf.cell(w=30, h=8, txt="Date: ", ln=0)
    pdf.cell(w=30, h=8, txt=date, ln=1)
    pdf.cell(w=30, h=8, txt="Author: ", ln=0)
    pdf.cell(w=30, h=8, txt=author, ln=1)

    pdf.ln(10)

    pdf.set_font(FONT, '', 14)

    pdf.cell(w=70, h=10, txt="Max height: ", ln=0)
    pdf.cell(w=70, h=10, txt=f'{max_height} m', ln=1)
    pdf.cell(w=70, h=10, txt="Max speed: ", ln=0)
    pdf.cell(w=70, h=10, txt=f'{max_speed} m/s', ln=1)
    pdf.cell(w=70, h=10, txt="Max Angle of Attack: ", ln=0)
    pdf.cell(w=70, h=10, txt=f'{max_angle_of_attack} °', ln=1)
    pdf.cell(w=70, h=10, txt="Displacement: ", ln=0)
    pdf.cell(w=70, h=10, txt=f'{displacement} m', ln=1)

    pdf.add_page()

    pdf.set_font(FONT, 'B', 24)
    pdf.cell(w=0, h=20, txt="Charts", ln=1, align="C")

    pdf.image(speedChart, x=30, y=None, w=150, h=0, type="PNG")
    pdf.image(heightChart, x=30, y=None, w=150, h=0, type="PNG")

    pdf.output(
        f"report_{datetime.now().strftime('%d-%m-%Y__%H_%M_%S')}.pdf", "F")


def generate_charts():
    pass


def main():
    parser = argparse.ArgumentParser(description='Generate a report.')
    parser.add_argument('filePath')
    args = parser.parse_args()
    filePath = args.filePath

    data = []

    with open(filePath, newline='') as csvfile:
        reader = csv.reader(csvfile, delimiter=',')

        for row in reader:
            data.append(row)

    generate_charts()
    generate_pdf(0, 0, 0, 0, date.today().strftime("%d/%m/%Y"),
                 "Filip Gawlik", "speedChart.png", "heightChart.png")


main()
