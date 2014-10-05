#!/usr/bin/env python

#Autor: Ivan Padron Dimas <ivpadim@gmail.com>
#Matricula: 678419
#Examen 3er Parcial

import wx

class MyFrame(wx.Frame):
    def __init__(self, parent, id, title):

        wx.Frame.__init__(self, parent, id, title, wx.DefaultPosition, wx.Size(300, 250))
        self.formula = False
        menubar = wx.MenuBar()
        file = wx.Menu()
        file.Append(22, '&Quit', 'Exit Calculator')
        menubar.Append(file, '&File')
        self.SetMenuBar(menubar)
        wx.EVT_MENU(self, 22, self.OnClose)
        sizer = wx.BoxSizer(wx.VERTICAL)
        self.display = wx.TextCtrl(self, -1, '',  style=wx.TE_RIGHT)
        sizer.Add(self.display, 0, wx.EXPAND | wx.TOP | wx.BOTTOM, 4)

        gs = wx.GridSizer(4, 5, 3, 3)
        gs.AddMany([(wx.Button(self, 21, 'Cls'), 0, wx.EXPAND),
                        (wx.Button(self, 22, 'Bck'), 0, wx.EXPAND),
                        (wx.StaticText(self, -1, ''), 0, wx.EXPAND),
                        (wx.StaticText(self, -1, ''), 0, wx.EXPAND),
                        (wx.Button(self, 23, 'Close'), 0, wx.EXPAND),
                        (wx.Button(self, 1, '7'), 0, wx.EXPAND),
                        (wx.Button(self, 2, '8'), 0, wx.EXPAND),
                        (wx.Button(self, 3, '9'), 0, wx.EXPAND),
                        (wx.Button(self, 4, '/'), 0, wx.EXPAND),
                        (wx.Button(self, 17, '1/x'), 0, wx.EXPAND),
                        (wx.Button(self, 5, '4'), 0, wx.EXPAND),
                        (wx.Button(self, 6, '5'), 0, wx.EXPAND),
                        (wx.Button(self, 7, '6'), 0, wx.EXPAND),
                        (wx.Button(self, 8, '*'), 0, wx.EXPAND),
                        (wx.Button(self, 18, 'x^2'), 0, wx.EXPAND),
                        (wx.Button(self, 9, '1'), 0, wx.EXPAND),
                        (wx.Button(self, 10, '2'), 0, wx.EXPAND),
                        (wx.Button(self, 11, '3'), 0, wx.EXPAND),
                        (wx.Button(self, 12, '-'), 0, wx.EXPAND),
                        (wx.Button(self, 19, 'x^y'), 0, wx.EXPAND),
                        (wx.Button(self, 13, '0'), 0, wx.EXPAND),
                        (wx.Button(self, 14, '.'), 0, wx.EXPAND),
                        (wx.Button(self, 15, '='), 0, wx.EXPAND),
                        (wx.Button(self, 16, '+'), 0, wx.EXPAND),
                        (wx.Button(self, 20, '%'), 0, wx.EXPAND)])

        sizer.Add(gs, 1, wx.EXPAND)

        self.SetSizer(sizer)
        self.Centre()

        self.Bind(wx.EVT_BUTTON, self.OnClear, id=21)
        self.Bind(wx.EVT_BUTTON, self.OnBackspace, id=22)
        self.Bind(wx.EVT_BUTTON, self.OnClose, id=23)
        self.Bind(wx.EVT_BUTTON, self.OnSeven, id=1)
        self.Bind(wx.EVT_BUTTON, self.OnEight, id=2)
        self.Bind(wx.EVT_BUTTON, self.OnNine, id=3)
        self.Bind(wx.EVT_BUTTON, self.OnDivide, id=4)
        self.Bind(wx.EVT_BUTTON, self.OnFour, id=5)
        self.Bind(wx.EVT_BUTTON, self.OnFive, id=6)
        self.Bind(wx.EVT_BUTTON, self.OnSix, id=7)
        self.Bind(wx.EVT_BUTTON, self.OnMultiply, id=8)
        self.Bind(wx.EVT_BUTTON, self.OnOne, id=9)
        self.Bind(wx.EVT_BUTTON, self.OnTwo, id=10)
        self.Bind(wx.EVT_BUTTON, self.OnThree, id=11)
        self.Bind(wx.EVT_BUTTON, self.OnMinus, id=12)
        self.Bind(wx.EVT_BUTTON, self.OnZero, id=13)
        self.Bind(wx.EVT_BUTTON, self.OnDot, id=14)
        self.Bind(wx.EVT_BUTTON, self.OnEqual, id=15)
        self.Bind(wx.EVT_BUTTON, self.OnPlus, id=16)
        self.Bind(wx.EVT_BUTTON, self.OnInverse, id=17)
        self.Bind(wx.EVT_BUTTON, self.OnPowerTwo, id=18)
        self.Bind(wx.EVT_BUTTON, self.OnPowerY, id=19)
        self.Bind(wx.EVT_BUTTON, self.OnPercent, id=20)

    def OnClear(self, event):
        self.display.Clear()

    def OnBackspace(self, event):
        formula = self.display.GetValue()
        self.display.Clear()
        self.display.SetValue(formula[:-1])

    def OnClose(self, event):
        self.Close()

    def OnDivide(self, event):
        if self.formula:
            return
        self.display.AppendText('/')

    def OnMultiply(self, event):
        if self.formula:
            return
        self.display.AppendText('*')

    def OnMinus(self, event):
        if self.formula:
            return
        self.display.AppendText('-')

    def OnPlus(self, event):
        if self.formula:
            return
        self.display.AppendText('+')

    def OnInverse(self, event):
        if self.formula:
            return
	value = self.display.GetValue()
	self.display.Clear()
        self.display.AppendText('1/' + value)

    def OnPowerTwo(self, event):
        if self.formula:
            return
        self.display.AppendText('^2')

    def OnPowerY(self, event):
        if self.formula:
            return
        self.display.AppendText('^')

    def OnPercent(self, event):
        if self.formula:
            return
        self.display.AppendText('%')

    def OnDot(self, event):
        if self.formula:
            return
        self.display.AppendText('.')

    def OnEqual(self, event):
        if self.formula:
            return
        formula = self.display.GetValue()
        self.formula = False
        try:
            self.display.Clear()
            formula = formula.replace('^','**')
            if(formula.endswith('%') == True):
		formula = formula.replace('%','') + '/100.0'
            output = eval(formula)
            self.display.AppendText(str(output))
        except StandardError:
            self.display.AppendText("Error")

    def OnZero(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('0')

    def OnOne(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('1')

    def OnTwo(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('2')

    def OnThree(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('3')

    def OnFour(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('4')

    def OnFive(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('5')

    def OnSix(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('6')

    def OnSeven(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('7')

    def OnEight(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('8')

    def OnNine(self, event):
        if self.formula:
            self.display.Clear()
            self.formula = False
        self.display.AppendText('9')

class MyApp(wx.App):
    def OnInit(self):
        frame = MyFrame(None, -1, 'calculator.py')
        frame.Show(True)
        self.SetTopWindow(frame)
        return True

app = MyApp(0)
app.MainLoop()
