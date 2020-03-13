latex-book-example.pdf : main.pdf
	ps2pdf $^ $@

main.pdf : main.tex structure.tex
	pdflatex main.tex

clean :
	$(RM) latex-book-example.pdf main.pdf
	$(RM) main.aux main.bcf main.log main.out main.pdf main.ptc main.run.xml main.toc
